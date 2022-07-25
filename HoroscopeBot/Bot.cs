using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Exceptions;
using HoroscopeBot.AHoroscope;
using HoroscopeBot.Aztro;
using HoroscopeBot.CHoroscope;
using HoroscopeBot.ChTranslate;

namespace HoroscopeBot
{
    public class Bot
    {
        TelegramBotClient botclient = new TelegramBotClient("5429233967:AAFy0RyxsUviEYbOXAIF0Gcty_nSslHZOLo");
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiveroptions = new ReceiverOptions { AllowedUpdates = { } };

        public async Task Start()
        {
            botclient.StartReceiving(HandlerUpdateAsync, HandlerError, receiveroptions, cancellationToken);
            var botMe = await botclient.GetMeAsync();
            Console.WriteLine("Бот почав працювати");
            
        }
        public string HoroType;
        public string period;
        public string Sign;
        public string SignDate;

        private Task HandlerError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"\nПомилка в телеграм бот АПІ :\n{apiRequestException.ErrorCode}\n" +
                $"\n{apiRequestException.Message}",_=> exception.ToString()
            };
            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if(update.Type==UpdateType.Message && update.Message.Text != null)
            {
                await HandlerMessageAsync(botClient, update.Message);
            }
        }

        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберіть команду /keyboard, щоб отримати ваш гороскоп" +
                    "\nВиберіть команду /allsignswestern, щоб отримати всі знаки західного гороскопу" +
                    "\nВиберіть команду /allsignschinese, щоб дізнатися всі знаки китайського гроскопу");
                
                ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                    new[]
                        {
                            new KeyboardButton[] {"/keyboard", "/allsignswestern", "/allsignschinese"}
                        }
                    )
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Оберіть дію ", replyMarkup: replyKeyboardMarkup);
                return;
            }
            if(message.Text == "/keyboard")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                    new[]
                        {
                            new KeyboardButton[] {"Китайський гороскоп", "Західний гороскоп", "Опис дня"}
                        }
                    )
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберіть пункт меню: ", replyMarkup: replyKeyboardMarkup);
                return;
            }
            if (message.Text == "/allsignswestern")
            {
                string allsigns = "Овен	Aries   21 березня — 20 квітня"

                                +"\nТелець  Taurus   21 квітня — 21 травня"

                                +"\nБлизнята     Gemini     22 травня — 21 червня"

                                +"\nРак     Cancer      22 червня — 22 липня"

                                +"\nЛев     Leo     23 липня — 22 серпня"

                                +"\nДіва     Virgo   23 серпня — 23 вересня"

                                +"\nТерези    Libra     24 вересня — 23 жовтня"

                                +"\nСкорпіон     Scorpio     24 жовтня — 22 листопада"

                                +"\nСтрілець    Sagittarius     23 листопада — 21 грудня"

                                +"\nКозоріг     Capricornus     22 грудня — 20 січня"

                                +"\nВодолій     Aquarius     21 січня — 18 лютого"

                                +"\nРиби    Pisces      19 лютого — 20 березня";
                await botclient.SendTextMessageAsync(message.Chat.Id, allsigns);
                return;
            }
            if (message.Text == "/allsignschinese")
            {
                string allsigns = "Щур   鼠年 (子)	2020, 2008, 1996, 1984, 1972, 1960, 1948, 1936"
                                    +"\nВіл    牛年(丑)    2021, 2009, 1997, 1985, 1973, 1961, 1949, 1937"
                                    +"\nТигр   虎年(寅)   2022, 2010, 1998, 1986, 1974, 1962, 1950, 1938"
                                    +"\nКролик     兔年(卯)  2011, 1999, 1987, 1975, 1963, 1951, 1939"
                                    +"\nДракон     龙年(辰)  2012, 2000, 1988, 1976, 1964, 1952, 1940"
                                    +"\nЗмія   蛇年(巳)   2013, 2001, 1989, 1977, 1965, 1953, 1941"
                                    +"\nКінь   马年(午)   2014, 2002, 1990, 1978, 1966, 1954, 1942"
                                    +"\nКоза   羊年(未)   2015, 2003, 1991, 1979, 1967, 1955, 1943"
                                    +"\nМавпа      猴年(申)   2016, 2004, 1992, 1980, 1968, 1956, 1944"
                                    +"\nПівень     鸡年(酉)  2017, 2005, 1993, 1981, 1969, 1957, 1945"
                                    +"\nСобака     狗年(戌)  2018, 2006, 1994, 1982, 1970, 1958, 1946"
                                    +"\nСвиня      猪年(亥)  2019, 2007, 1995, 1983, 1971, 1959, 1947";
                await botclient.SendTextMessageAsync(message.Chat.Id, allsigns);
                return;
            }
            if (message.Text == "Західний гороскоп")
            {
                HoroType = "Західний гороскоп";
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Введіть ваш знак зодіаку та період часу (місяць, тиждень, день) Приклад вводу : водолій, завтра ") ;
                return;
            }
            if (message.Text == "Опис дня")
            {
                HoroType = "Опис дня";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Введіть ваш знак зодіаку та день");
                return;
            }
            if(message.Text=="Китайський гороскоп")
            {
                HoroType = "Китайський гороскоп";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Введіть ваш знак зодіаку та період часу (місяць/ тиждень/ день)");
                return;
            }
            ChTranslateClient chTranslateClient = new ChTranslateClient();

            if (HoroType == "Західний гороскоп")
            {
                try
                {
                    string result = "";
                    AClient aclient = new AClient();
                    string[] needenparms = message.Text.Split(", ");
                    string SignUkr = needenparms[0];
                    string period = needenparms[1];
                    if (CheckPeriod(period) == true && CheckWSign(SignUkr) == true)
                    {
                        switch (SignUkr)
                        {
                            case "овен":
                                Sign = "aries";
                                result = aclient.GetAHoro(Sign, period).Result.aries.ToString();
                                break;
                            case "тілець":
                                Sign = "taurus";
                                result = aclient.GetAHoro(Sign, period).Result.taurus.ToString();
                                break;
                            case "близнюки":
                                Sign = "gemini";
                                result = aclient.GetAHoro(Sign, period).Result.gemini.ToString();
                                break;
                            case "рак":
                                Sign = "cancer";
                                result = aclient.GetAHoro(Sign, period).Result.cancer.ToString();
                                break;
                            case "лев":
                                Sign = "leo";
                                result = aclient.GetAHoro(Sign, period).Result.leo.ToString();
                                break;
                            case "діва":
                                Sign = "virgo";
                                result = aclient.GetAHoro(Sign, period).Result.virgo.ToString();
                                break;
                            case "терези":
                                Sign = "libra";
                                result = aclient.GetAHoro(Sign, period).Result.libra.ToString();
                                break;
                            case "стрілець":
                                Sign = "sagittarius";
                                result = aclient.GetAHoro(Sign, period).Result.sagittarius.ToString();
                                break;
                            case "скорпіон":
                                Sign = "scorpio";
                                result = aclient.GetAHoro(Sign, period).Result.scorpio.ToString();
                                break;
                            case "водолій":
                                Sign = "aquarius";
                                result = aclient.GetAHoro(Sign, period).Result.aquarius.ToString();
                                break;
                            case "риби":
                                Sign = "pisces";
                                result = aclient.GetAHoro(Sign, period).Result.pisces.ToString();
                                break;
                            case "козеріг":
                                Sign = "capricorn";
                                result = aclient.GetAHoro(Sign, period).Result.capricorn.ToString();
                                break;
                        }

                        string tresult = "";
                        string[] sents = result.Split('.', '!', '?');
                        foreach (string s in sents)
                        {
                            try
                            {
                                tresult += chTranslateClient.CheapTranslate("en", s, "uk").Result.translatedText;
                                tresult += ".";
                            }
                            catch (Exception)
                            {
                                break;
                                //continue;
                            }
                        }
                        await botClient.SendTextMessageAsync(message.Chat.Id, tresult);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Щось пішло не так, перевірте правильність вводу");
                    }
                    return;
                }
                catch (Exception)
                {
                    await botclient.SendTextMessageAsync(message.Chat.Id, "Щось пішло не так, перевірте правильність вводу");
                }
            }
            else if (HoroType == "Опис дня")
            {
                try
                {
                    Aztro_Client aztroclient = new Aztro_Client();
                    string[] neededparms = message.Text.Split(", ");//ukr
                    string SignUkr = neededparms[0];//ukr
                    period = neededparms[1];
                    if (CheckPeriod(period) == true && CheckWSign(SignUkr) == true)
                    {
                        switch (SignUkr)
                        {
                            case "овен":
                                Sign = "aries";
                                break;
                            case "тілець":
                                Sign = "taurus";
                                break;
                            case "близнюки":
                                Sign = "gemini";
                                break;
                            case "рак":
                                Sign = "cancer";
                                break;
                            case "лев":
                                Sign = "leo";
                                break;
                            case "діва":
                                Sign = "virgo";
                                break;
                            case "терези":
                                Sign = "libra";
                                break;
                            case "стрілець":
                                Sign = "sagittarius";
                                break;
                            case "скорпіон":
                                Sign = "scorpio";
                                break;
                            case "водолій":
                                Sign = "aquarius";
                                break;
                            case "риби":
                                Sign = "pisces";
                                break;
                            case "козеріг":
                                Sign = "capricorn";
                                break;
                        }
                        try
                        {
                            period = chTranslateClient.CheapTranslate("uk", neededparms[1], "en").Result.translatedText;//ukr becmes eng
                            var result = "Description: " + aztroclient.GetHoro(Sign, period).Result.description;//eng
                            result += "\n" + "Mood :" + aztroclient.GetHoro(Sign, period).Result.mood;
                            result += "\n" + "Lucky number :" + aztroclient.GetHoro(Sign, period).Result.lucky_number;
                            result += "\n" + "Lucky time :" + aztroclient.GetHoro(Sign, period).Result.lucky_time;
                            result += "\n" + "Compatability :" + aztroclient.GetHoro(Sign, period).Result.compatibility;
                            string trresult = chTranslateClient.CheapTranslate("en", result, "uk").Result.translatedText;//result ukr
                            await botClient.SendTextMessageAsync(message.Chat.Id, trresult);
                        }
                        catch (Exception)
                        {
                            await botclient.SendTextMessageAsync(message.Chat.Id, "Перевищено кількість запросів для перекладача, зачекайте хвилинку");
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Щось пішло не так, перевірте правильність вводу");
                    }
                }
                catch (Exception)
                {
                    await botclient.SendTextMessageAsync(message.Chat.Id, "Щось пішло не так, перевірте правильність вводу");
                }
            }
            else if (HoroType == "Китайський гороскоп")
            {
                try {
                    string result = "";
                    CClient cclient = new CClient();
                    string[] neededparms = message.Text.Split(", ");//in ukrainian
                    try
                    {
                        if (neededparms[0] == "віл")
                        {
                            Sign = "ox";
                        }
                        else
                        {
                            Sign = chTranslateClient.CheapTranslate("uk", neededparms[0], "en").Result.translatedText;
                        }
                    }
                    catch (Exception)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Перевищено кількість запросів для перекладача, зачекайте хвилинку");
                    }
                    period = neededparms[1];


                    if (CheckPeriod(period) == true && CheckCSign(Sign) == true)
                    {
                        try
                        {
                            switch (Sign)
                            {
                                case "ox":
                                    result = cclient.GetCHoro(Sign, period).Result.Ox.ToString();
                                    break;
                                case "tiger":
                                    result = cclient.GetCHoro(Sign, period).Result.Tiger.ToString();
                                    break;
                                case "rabbit":
                                    result = cclient.GetCHoro(Sign, period).Result.Rabbit.ToString();
                                    break;
                                case "dragon":
                                    result = cclient.GetCHoro(Sign, period).Result.Dragon.ToString();
                                    break;
                                case "snake":
                                    result = cclient.GetCHoro(Sign, period).Result.Snake.ToString();
                                    break;
                                case "horse":
                                    result = cclient.GetCHoro(Sign, period).Result.Horse.ToString();
                                    break;
                                case "goat":
                                    result = cclient.GetCHoro(Sign, period).Result.Goat.ToString();
                                    break;
                                case "monkey":
                                    result = cclient.GetCHoro(Sign, period).Result.Monkey.ToString();
                                    break;
                                case "rooster":
                                    result = cclient.GetCHoro(Sign, period).Result.Rooster.ToString();
                                    break;
                                case "dog":
                                    result = cclient.GetCHoro(Sign, period).Result.Dog.ToString();
                                    break;
                                case "pig":
                                    result = cclient.GetCHoro(Sign, period).Result.Pig.ToString();
                                    break;
                                case "rat":
                                    result = cclient.GetCHoro(Sign, period).Result.Rat.ToString();
                                    break;
                            }

                            string trresult = chTranslateClient.CheapTranslate("en", result, "uk").Result.translatedText;
                            await botClient.SendTextMessageAsync(message.Chat.Id, trresult);
                        }
                        catch (Exception)
                        {
                            await botclient.SendTextMessageAsync(message.Chat.Id, "Помилка з сервером, спробуйте будь ласка ще раз пізніше");
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Щось пішло не так, перевірте правильність вводу");
                    }
                }
                catch (Exception)
                {
                    await botclient.SendTextMessageAsync(message.Chat.Id, "Щось пішло не так, перевірте правильність вводу");
                }
            }

        }
        public bool CheckPeriod(string p)
        {
            if(p=="місяць"|| p == "тиждень" || p == "сьогодні" || p == "вчора" || p == "завтра")
            {
                return true;
            }
            return false;
        }
        public bool CheckWSign(string s)
        {
            if(s=="овен"|| s == "тілець" || s == "близнюки" || s == "рак" || s == "лев" || s == "діва" || s == "терези" || s == "стрілець" || s == "скорпіон" || s == "козеріг" || s == "водолій" || s == "риби")
            {
                return true;
            }
            return false;
        }
        public bool CheckCSign(string s)
        {
            if(s == "ox" || s == "tiger" || s == "rabbit" || s == "dragon" || s == "snake" || s == "horse" || s == "goat" || s == "monkey" || s == "roster" || s == "dog" || s == "pig" || s == "rat")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
