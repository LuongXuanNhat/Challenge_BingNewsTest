using BingNew.DI;
using Newtonsoft.Json.Linq;

namespace NewsAggregationTest
{
    public class BingNewDITest
    {
        private readonly DIContainer _container = new();
 

        [Fact]
        public void DI_Register_Success()
        {
            _container.Register<IEmailService, YahooService>();
            var emailService = _container.Resolve<IEmailService>();
            Assert.NotNull(emailService);

            var mail = emailService.SendMail();
            Assert.NotNull(mail);
        }
        [Fact]
        public void DI_Register_Success_Of_Case_Not_Contructor()
        {
            _container.Register<IEmailService, OutlookService>(DIContainer.Lifetime.Singleton);
            var service = _container.Resolve<IEmailService>();
            Assert.NotNull(service);

            var mail = service.SendMail();
            Assert.NotNull(mail);
        }
        [Fact]
        public void Register_Scrope_DI_Success()
        {
            _container.Register<IEmailService, YahooService>(DIContainer.Lifetime.Scoped);
            var emailService = _container.Resolve<IEmailService>();
            Assert.NotNull(emailService);

            var mail = emailService.SendMail();
            Assert.NotNull(mail);
        }
        [Fact]
        public void Register_Singleton_DI_Success()
        {
            _container.Register<IEmailService, YahooService>(DIContainer.Lifetime.Singleton);
            var emailService = _container.Resolve<IEmailService>();
            Assert.NotNull(emailService);

            var mail = emailService.SendMail();
            Assert.NotNull(mail);
        }
        [Fact]
        public void DI_Register_Success_When_Obj_Implement_Have_Contructor()
        {
            _container.Register<IEmailService, YahooService>(DIContainer.Lifetime.Singleton);
            var yahooService = _container.Resolve<IEmailService>(typeof(YahooService).Name);
            Assert.NotNull(yahooService);

            var yahoo = yahooService.SendMail();
            Assert.NotNull(yahoo);
        }
        [Fact]
        public void DI_Register_Success_Of_Case_With_Multiple_Dependencies()
        {
            _container.Register<IEmailService, YahooService>();
            var yahooService = _container.Resolve<IEmailService>(typeof(YahooService).Name);
            _container.Register<IEmailService, OutlookService>();
            var emailService = _container.Resolve<IEmailService>();

            Assert.NotNull(emailService);
            Assert.NotNull(yahooService);

            var email = emailService.SendMail();
            var yahoo = yahooService.SendMail();

            Assert.NotNull(email);
            Assert.NotNull(yahoo);
        }
        [Fact]
        public void DI_Register_Success_Of_Case_Have_Contructor_And_Param_Is_Other_Dependency_Object()
        {
            _container.Register<IEmailService, YahooService>();
            _container.Register<IAccountService, People>(DIContainer.Lifetime.Singleton);
            var letter = _container.Resolve<IAccountService>();
            Assert.NotNull(letter);

            var result = letter.WriteLetter();
            Assert.NotNull(result);
        }
        [Fact]
        public void DI_Register_Success_Of_Case_Has_Nested_Constructor_With_Another_Dependency()
        {
            _container.Register<IAccountService, Account>();
            _container.Register<IEmailService, GmailService>();

            var email = _container.Resolve<IEmailService>();
            var result = email.SendMail();
            Assert.NotNull(result);

            var account = _container.Resolve<IAccountService>();
            var result2 = account.WriteLetter("Hello");
            Assert.NotNull(result2);
        }

        [Fact]
        public void DI_Register_Multiple_And_Has_Nested_Constructor_With_Another_Dependency()
        {
            _container.Register<IAccountService, Account>();
            _container.Register<IEmailService, OutlookService>();
            _container.Register<IEmailService, GmailService>();
            _container.Register<IEmailService, YahooService>();

            var gmail = _container.Resolve<IEmailService>(typeof(GmailService).Name);
            var yahoo = _container.Resolve<IEmailService>(typeof(GmailService).Name);
            var outlook = _container.Resolve<IEmailService>(typeof(GmailService).Name);

            var result1 = gmail.SendMail();
            var result2 = yahoo.SendMail();
            var result3 = outlook.SendMail();

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotNull(result3);
        }


    }

    public class OutlookService : IEmailService
    {
        public string SendMail()
        {
            return "Sent a letter";
        }
    }

    public class People : IAccountService
    {
        private readonly IEmailService _yahooService;
        public People(IEmailService service) {
            _yahooService = service;
        }

        public string WriteLetter(string content)
        {
            throw new NotImplementedException();
        }

        public string WriteLetter()
        {
            var getContent = _yahooService.SendMail();
            return getContent;
        }
    }

    public class Account : IAccountService
    {
        private readonly IEmailService _emailService;
        
        
        public Account(IEmailService emailService) {
            _emailService = emailService;
        }
        public string WriteLetter(string content)
        {
            var result = _emailService.SendMail();
            return $"{result} with content: {content}";
        }

        public string WriteLetter()
        {
            return "1";
        }
    }
    public interface IAccountService
    {
        string WriteLetter(string content);
        string WriteLetter();
    }
    public class GmailService : IEmailService
    {
        private readonly IAccountService _accountService;
        public GmailService(IAccountService account)
        {
            _accountService = account;
        }
        public string SendMail()
        {
            var result = _accountService.WriteLetter();
            return "Sent a letter " + result;
        }
    }
    public class YahooService : IEmailService
    {
        public int year;
        public YahooService(int year)
        {
            this.year = year;
        }

        public string SendMail()
        {
            return $"Yahoo was founded in 1994 - ";
        }
    }
    public interface IEmailService
    {
        string SendMail();
    }
}
