using Infrastructure.Contexts;

Console.WriteLine("Hello, World!");

var hc = new HttpClient();
hc.DefaultRequestHeaders.UserAgent.Clear();
hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
hc.DefaultRequestHeaders.Add("Accept", "*/*");
hc.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9");
hc.DefaultRequestHeaders.Add("Cookie",
"mda_exp_enabled=1; gdpr=0; yandex_login=Abopatau; i=tqq3YUVdxVO3f+wFD6zFX0wRPSzTMg+eKNXnfavs36wEwjL7zvH1AwKLfjrYjz/MKucZcZS/PlXJGvNdwDREwpGOb84=; yandexuid=5966005631677683065; yuidss=5966005631677683065; my_perpages=%5B%5D; L=ShBIc3R7cAcARmhQTV5dYmF2AFxkX2JhLRodFiAwOC8=.1682080411.15319.311325.3ddfd841fc8288584527022389845423; location=1; coockoos=4; users_info[check_sh_bool]=none; _ym_uid=1695210982988149803; crookie=H0YC9G8RqntJ1lU5qyRrI57/xw8oh6DM3Ly2OpA28ogSRczxF/alEyVyzyeFuoP7cggYoiRxUnVi2brul/Krs9ydCe8=; no-re-registration-required=1; _csrf=9Kc7DK3fxLpcddDpmTJcMI0h; disable_server_sso_redirect=1; _yasc=uEJsWPcJvPaOYFe2TJFH2rKZg+9Yk2sJ7Gk7zLdnFU6U00pY2itCDf8TwJZzyqNf4fE=; ya_sess_id=3:1699266252.5.0.1680186626131:kf5qTg:4d.1.2:1|1772340470.1893785.2.1:334267249.2:1893785.3:1682080411|30:10220185.4385.kKc9OXSYgTw7kQgJEMPM8Fw33Hk; sessar=1.1183.CiAXLffaZsSn9avIRC4g4pRnaPvnSbPLXhFfrbCukLz5rQ.L9fdvGfktG0CIAN8HTxJhg4yXXhDJFQTQ0YeYZrnRo8; ys=udn.cDpBYm9wYXRhdQ%3D%3D#c_chck.4085304732; mda2_beacon=1699266252723; sso_status=sso.passport.yandex.ru:synchronized; _ym_isad=1; PHPSESSID=60e6609c0eb163e52e90951581a23712; user_country=ru; yandex_gid=35; tc=431; uid=154156118; desktop_session_key=08af95be776afda07e101a6329650eb89cde30b9a369395b9d902d78a8520ac1e94cd97e8be57cf03acb852e0c298ccb42020b3de80f432654f6fe3e2825b294a8fa10e57c77f06124b2c915e29ab382cc03029e40e2c0bdb639008c64cf6501; desktop_session_key.sig=dB2dJPehvDc7tWUXTJXHz3wTffE; yp=1699352655.yu.5966005631677683065; ymex=1701858255.oyu.5966005631677683065; _ym_d=1699266286; kdetect=1"
);
hc.DefaultRequestHeaders.Add("Origin", "https://www.kinopoisk.ru");
bool stop = false;
while (!stop)
{
    using var context = new ApplicationDbContext();
}