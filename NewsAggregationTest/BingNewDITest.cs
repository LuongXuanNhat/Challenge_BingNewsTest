using BingNew.DI;

namespace NewsAggregationTest
{
    public class BingNewDITest
    {
        private readonly DIContainer _container = new();
 

        [Fact]
        public void Register_DI_Success()
        {
            _container.Register<IEmailService, GmailService>();
            var emailService = _container.Resolve<IEmailService>();
            Assert.NotNull(emailService);

            var mail = emailService.SendMail();
            Assert.NotNull(mail);
        }
        [Fact]
        public void Register_Scrope_DI_Success()
        {
            _container.Register<IEmailService, GmailService>(DIContainer.Lifetime.Scoped);
            var emailService = _container.Resolve<IEmailService>();
            Assert.NotNull(emailService);

            var mail = emailService.SendMail();
            Assert.NotNull(mail);
        }
        [Fact]
        public void Register_Singleton_DI_Success()
        {
            _container.Register<IEmailService, GmailService>(DIContainer.Lifetime.Singleton);
            var emailService = _container.Resolve<IEmailService>();
            Assert.NotNull(emailService);

            var mail = emailService.SendMail();
            Assert.NotNull(mail);
        }
        [Fact]
        public void Register_DI_Success_When_Obj_Implement_Have_Contructor()
        {
            _container.Register<IEmailService, YahooService>(DIContainer.Lifetime.Singleton);
            var yahooService = _container.Resolve<IEmailService>(typeof(YahooService).Name);
            Assert.NotNull(yahooService);

            var yahoo = yahooService.SendMail();
            Assert.NotNull(yahoo);
        }
        [Fact]
        public void Register_DI_Success_Of_Case_With_Multiple_Dependencies()
        {
            _container.Register<IEmailService, YahooService>();
            var yahooService = _container.Resolve<IEmailService>(typeof(YahooService).Name);
            _container.Register<IEmailService, GmailService>();
            var emailService = _container.Resolve<IEmailService>();

            Assert.NotNull(emailService);
            Assert.NotNull(yahooService);

            var email = emailService.SendMail();
            var yahoo = yahooService.SendMail();

            Assert.NotNull(email);
            Assert.NotNull(yahoo);
        }

        [Fact]
        public void Register_DI_Success_Of_Case_Have_Contructor_And_Param_Is_Other_Dependency_Object()
        {
            _container.Register<IAccountService, Account>(DIContainer.Lifetime.Singleton);
            var yahooService = _container.Resolve<IAccountService>();
            Assert.NotNull(yahooService);

            var yahoo = yahooService.WriteLetter("Happy Day: 20/20/2020");
            Assert.NotNull(yahoo);
        }

    }

    public class Account : IAccountService
    {
        private readonly DIContainer _container = new();
        private readonly IEmailService _emailService;
        
        
        public Account(IEmailService emailService) {
            _emailService = emailService;
        }
        public string WriteLetter(string content)
        {
            var result = _emailService.SendMail();
            return $"{result} with content: {content}";
        }


    }

    public interface IAccountService
    {
        string WriteLetter(string content);
    }

    public class YahooService : IEmailService
    {
        public int created;
        public YahooService(int year) {
            created = year;
        }
        public string SendMail()
        {
            return $"Yahoo was founded in {created}";
        }
    }
    public class GmailService : IEmailService
    {
        private IAccountService _accountService;
        public GmailService
        public string SendMail()
        {
            return "Sent a letter";
        }
    }
    public interface IEmailService
    {
        string SendMail();
    }
}
