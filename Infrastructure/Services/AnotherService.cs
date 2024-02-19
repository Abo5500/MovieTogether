using Application.Interfaces;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieTogether.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AnotherService: IAnotherService
    {
        private readonly IServiceProvider _serviceProvider;

        public AnotherService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<bool> RemoveAllActors()
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Actors.ExecuteDeleteAsync();
            return true;
        }
        public async Task<bool> FindDirectors()
        {
            var hc = new HttpClient();
            hc.DefaultRequestHeaders.UserAgent.Clear();
            hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36");
            hc.DefaultRequestHeaders.Add("Accept", "*/*");
            hc.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9");
            hc.DefaultRequestHeaders.Add("Cookie",
                "mda_exp_enabled=1; gdpr=0; yandex_login=Abopatau; i=tqq3YUVdxVO3f+wFD6zFX0wRPSzTMg+eKNXnfavs36wEwjL7zvH1AwKLfjrYjz/MKucZcZS/PlXJGvNdwDREwpGOb84=; yandexuid=5966005631677683065; yuidss=5966005631677683065; my_perpages=%5B%5D; L=ShBIc3R7cAcARmhQTV5dYmF2AFxkX2JhLRodFiAwOC8=.1682080411.15319.311325.3ddfd841fc8288584527022389845423; location=1; coockoos=4; users_info[check_sh_bool]=none; no-re-registration-required=1; _csrf=9Kc7DK3fxLpcddDpmTJcMI0h; _ym_isad=1; PHPSESSID=60e6609c0eb163e52e90951581a23712; user_country=ru; yandex_gid=35; tc=431; uid=154156118; desktop_session_key=08af95be776afda07e101a6329650eb89cde30b9a369395b9d902d78a8520ac1e94cd97e8be57cf03acb852e0c298ccb42020b3de80f432654f6fe3e2825b294a8fa10e57c77f06124b2c915e29ab382cc03029e40e2c0bdb639008c64cf6501; desktop_session_key.sig=dB2dJPehvDc7tWUXTJXHz3wTffE; yp=1699352655.yu.5966005631677683065; ymex=1701858255.oyu.5966005631677683065; _csrf_csrf_token=uRFa-_G4uU9Gp1bz8DS4PaQNuiEWOSd6tLoQSuDG0so; mobile=no; ya_sess_id=3:1699279293.5.0.1680186626131:kf5qTg:4d.1.2:1|1772340470.1893785.2.1:334267249.2:1893785.3:1682080411|30:10220188.572564.PB0eq8CbQAAlGbC_qs6V5Q4chFM; sessar=1.1183.CiBY940-CJ31kzYdlZDujQzqcXbo0uQ-0voP_eeNHOmTMg.X6w59UVaBVJ_3SF9tmfQJyC4CKtk1-ez8yTkypwnArY; ys=udn.cDpBYm9wYXRhdQ%3D%3D#c_chck.2906101693; mda2_beacon=1699279293068; sso_status=sso.passport.yandex.ru:synchronized; crookie=yoVWkobn88RqIYMZmAQh8nUCCOeT7TIf/D8Mb6O8g4/qFQ9SLaDCaa8bIqBTGp5U0sSWDSwqr9EonPihEbFdltOacnw=; cmtchd=MTY5OTI3OTI5NzUxNw==; _ym_uid=1695210982988149803; _ym_visorc=b; kdetect=1; _yasc=tsH8KlJoJfpZ2/r+uobtL1waGrehAOP98spofjwuivtogyjPP55rwp9aeUqaVLHUegZH2oUQebg=; spravka=dD0xNjk5Mjg0MDYxO2k9OTUuMjUuMTQyLjE0O0Q9QTM1OEZBRTQ3M0RCQzIxNzY4OTk0NTY2Nzk0RkEzRkM4QzgyMjNGNDkwM0Y2MDRGMDlBQTA0RERCNEVBMzdBODYyOEI2NzdCNENFMkU3Rjc5NENBOTFDMkVGMzM0MkFDQzQwMThEQTIyQzE5OEI3RjU5Q0MzQ0JBRDIwNjk2MENFRTI2MzEwREZBQUZEQ0MyOEMzRkU5MEMyQjlGNTlCNDg3RjQ3M0Q3RDgwQUVCMzlENTt1PTE2OTkyODQwNjE1MzIxNzAxNzM7aD02NGViMjc3MDUxYTU1NjkwYTc4Yzg3YjFkNmIxN2FkNg==; kpunk=1; _ym_d=1699284200"
                );
            hc.DefaultRequestHeaders.Add("Origin", "https://www.kinopoisk.ru");
            hc.DefaultRequestHeaders.Add("Referer", "https://www.kinopoisk.ru/");
            try
            {
                using var scope = _serviceProvider.CreateAsyncScope();
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var ddirector = await context.Directors.FirstOrDefaultAsync(x => x.FullName == "None");
                var movies = await context.Movies.Include(x => x.Directors).Where(x => x.Directors.Contains(ddirector)).ToListAsync();
                Console.WriteLine($"\n\n\n Start wave, {movies.Count}");
                if (movies.Count == 0) { return true; }
                ConcurrentBag<Director> concurrentDirectors = new ConcurrentBag<Director>(await context.Directors.ToListAsync());

                ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 30 };
                Parallel.ForEach(movies, parallelOptions, movie =>
                {
                    List<Director> directors = DirectorsWork(movie.KinopoiskId, hc).Result;
                    movie.Directors = new();
                    
                    foreach (var director in directors)
                    {
                        var concDirector = concurrentDirectors.FirstOrDefault(x => x.FullName == director.FullName);
                        if (concDirector != null)
                        {
                            movie.Directors.Add(concDirector);
                        }
                        else
                        {
                            concurrentDirectors.Add(director);
                            movie.Directors.Add(director);
                        }
                    }
                });
                context.Movies.UpdateRange(movies);
                await context.SaveChangesAsync();
                Console.WriteLine($"\n\n\n Success for wave, save {movies.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }
        private async Task<List<Director>> DirectorsWork(int kpId, HttpClient httpClient)
        {
            string url = $"https://www.kinopoisk.ru/film/{kpId}/";
            var response = await httpClient.GetAsync(url);
            string html = await response.Content.ReadAsStringAsync();
            if (html.Length < 325947)
            {
                Console.WriteLine("CAPTCHA CAPTCHA CAPTCHA");
                return new();
            }
            int x = html.IndexOf("Режиссер");
            int y = html.IndexOf("Сценарий");
            if(x == -1)
            {
                return new() { new Director { FullName = "None" } };
            }
            if (y == -1)
            {
                y = x + 1000;
            }
            html = html.Substring(x, y-x);
            Regex regex = new Regex(@"href=""/name/([^/]*)[^>]*>([^<]*)</a>");
            MatchCollection matches = regex.Matches(html);
            if (matches.Count == 0)
            {
                return new() { new Director { FullName = "None"} };
            }
            List<Director> directors = new List<Director>();
            if (matches.Count >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    directors.Add(new Director()
                    {
                        FullName = matches[i].Groups[2].Value
                    });
                }
            }
            else
            {
                foreach (var match in matches.Cast<Match>())
                {
                    directors.Add(new Director()
                    {
                        FullName = match.Groups[2].Value
                    });
                }
            }

            return directors;
        }
        public async Task<bool> StartWork()
        {
            var hc = new HttpClient();
            hc.DefaultRequestHeaders.UserAgent.Clear();
            hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36");
            hc.DefaultRequestHeaders.Add("Accept", "*/*");
            hc.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9");
            hc.DefaultRequestHeaders.Add("Cookie",
                "mda_exp_enabled=1; gdpr=0; yandex_login=Abopatau; i=tqq3YUVdxVO3f+wFD6zFX0wRPSzTMg+eKNXnfavs36wEwjL7zvH1AwKLfjrYjz/MKucZcZS/PlXJGvNdwDREwpGOb84=; yandexuid=5966005631677683065; yuidss=5966005631677683065; my_perpages=%5B%5D; L=ShBIc3R7cAcARmhQTV5dYmF2AFxkX2JhLRodFiAwOC8=.1682080411.15319.311325.3ddfd841fc8288584527022389845423; location=1; coockoos=4; users_info[check_sh_bool]=none; no-re-registration-required=1; _csrf=9Kc7DK3fxLpcddDpmTJcMI0h; _ym_isad=1; PHPSESSID=60e6609c0eb163e52e90951581a23712; user_country=ru; yandex_gid=35; tc=431; uid=154156118; desktop_session_key=08af95be776afda07e101a6329650eb89cde30b9a369395b9d902d78a8520ac1e94cd97e8be57cf03acb852e0c298ccb42020b3de80f432654f6fe3e2825b294a8fa10e57c77f06124b2c915e29ab382cc03029e40e2c0bdb639008c64cf6501; desktop_session_key.sig=dB2dJPehvDc7tWUXTJXHz3wTffE; yp=1699352655.yu.5966005631677683065; ymex=1701858255.oyu.5966005631677683065; _csrf_csrf_token=uRFa-_G4uU9Gp1bz8DS4PaQNuiEWOSd6tLoQSuDG0so; mobile=no; ya_sess_id=3:1699279293.5.0.1680186626131:kf5qTg:4d.1.2:1|1772340470.1893785.2.1:334267249.2:1893785.3:1682080411|30:10220188.572564.PB0eq8CbQAAlGbC_qs6V5Q4chFM; sessar=1.1183.CiBY940-CJ31kzYdlZDujQzqcXbo0uQ-0voP_eeNHOmTMg.X6w59UVaBVJ_3SF9tmfQJyC4CKtk1-ez8yTkypwnArY; ys=udn.cDpBYm9wYXRhdQ%3D%3D#c_chck.2906101693; mda2_beacon=1699279293068; sso_status=sso.passport.yandex.ru:synchronized; crookie=yoVWkobn88RqIYMZmAQh8nUCCOeT7TIf/D8Mb6O8g4/qFQ9SLaDCaa8bIqBTGp5U0sSWDSwqr9EonPihEbFdltOacnw=; cmtchd=MTY5OTI3OTI5NzUxNw==; _ym_uid=1695210982988149803; _ym_visorc=b; kdetect=1; _yasc=tsH8KlJoJfpZ2/r+uobtL1waGrehAOP98spofjwuivtogyjPP55rwp9aeUqaVLHUegZH2oUQebg=; spravka=dD0xNjk5Mjg0MDYxO2k9OTUuMjUuMTQyLjE0O0Q9QTM1OEZBRTQ3M0RCQzIxNzY4OTk0NTY2Nzk0RkEzRkM4QzgyMjNGNDkwM0Y2MDRGMDlBQTA0RERCNEVBMzdBODYyOEI2NzdCNENFMkU3Rjc5NENBOTFDMkVGMzM0MkFDQzQwMThEQTIyQzE5OEI3RjU5Q0MzQ0JBRDIwNjk2MENFRTI2MzEwREZBQUZEQ0MyOEMzRkU5MEMyQjlGNTlCNDg3RjQ3M0Q3RDgwQUVCMzlENTt1PTE2OTkyODQwNjE1MzIxNzAxNzM7aD02NGViMjc3MDUxYTU1NjkwYTc4Yzg3YjFkNmIxN2FkNg==; kpunk=1; _ym_d=1699284200"
                );
            hc.DefaultRequestHeaders.Add("Origin", "https://www.kinopoisk.ru");
            hc.DefaultRequestHeaders.Add("Referer", "https://www.kinopoisk.ru/");
            bool stop = false;
            while (!stop)
            {
                try
                {
                    using var scope = _serviceProvider.CreateAsyncScope();
                    using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var movies = await context.Movies.Where(x => !x.Actors.Any()).Take(500).ToListAsync();
                    Console.WriteLine($"\n\n\n Start wave, {movies.Count}");
                    if (movies.Count == 0) { stop = true; }
                    ConcurrentBag<Actor> concurrentActors = new ConcurrentBag<Actor>(await context.Actors.ToListAsync());
                    
                    ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 30 };
                    Parallel.ForEach(movies, parallelOptions, movie =>
                    {
                        List<Actor> actors = Work(movie.KinopoiskId, hc).Result;
                        movie.Actors = new();
                        
                        foreach (var actor in actors)
                        {
                            var concActor = concurrentActors.FirstOrDefault(x => x.KinopoiskId == actor.KinopoiskId);
                            if (concActor != null)
                            {
                                movie.Actors.Add(concActor);
                            }
                            else
                            {
                                concurrentActors.Add(actor);
                                movie.Actors.Add(actor);
                            }
                        }
                    });
                    context.Movies.UpdateRange(movies);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"\n\n\n Success for wave, save {movies.Count}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return true;
        }
        public async Task<bool> RemoveNoneActor()
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.FullName == "None");
            context.Actors.Remove(actor);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RenameCharsActors()
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var actors = await context.Actors.Where(x => x.FullName.Contains("&#x27;")).ToListAsync();
            foreach (var actor in actors)
            {
                actor.FullName = actor.FullName.Replace("&#x27;", "'");
            }
            context.Actors.UpdateRange(actors);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RenameCharsDirectors()
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var directors = await context.Directors.Where(x => x.FullName.Contains("&#x27;")).ToListAsync();
            foreach (var director in directors)
            {
                director.FullName = director.FullName.Replace("&#x27;", "'");
            }
            context.Directors.UpdateRange(directors);
            await context.SaveChangesAsync();
            return true;
        }
        private async Task<List<Actor>> Work(int kpId, HttpClient httpClient)
        {
            string url = $"https://www.kinopoisk.ru/film/{kpId}/";
            var response = await httpClient.GetAsync(url);
            string html = await response.Content.ReadAsStringAsync();
            if(html.Length < 325947)
            {
                Console.WriteLine("CAPTCHA CAPTCHA CAPTCHA");
                return new();
            }
            Regex regex = new Regex(@"<a href=""/name/([^/]*)[^>]*>([^<]*)</a>");
            MatchCollection matches = regex.Matches(html);
            if (matches.Count == 0)
            {
                return new() { new Actor { FullName = "None", KinopoiskId = -1 } };
            }
            List<Actor> actors = new List<Actor>();
            if(matches.Count >= 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    actors.Add(new Actor()
                    {
                        KinopoiskId = int.Parse(matches[i].Groups[1].Value),
                        FullName = matches[i].Groups[2].Value
                    });
                }
            }
            else
            {
                foreach(var match in matches.Cast<Match>())
                {
                    actors.Add(new Actor()
                    {
                        KinopoiskId = int.Parse(match.Groups[1].Value),
                        FullName = match.Groups[2].Value
                    });
                }
            }

            return actors;
        }
    }
}
