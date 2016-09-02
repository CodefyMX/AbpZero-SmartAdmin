using Abp.Dependency;
using SendGrid;
using System;

namespace Cinotam.MailSender.SendGrid.Credentials
{
    public interface ISendGridCredentialsService : ISingletonDependency
    {
        SendGridAPIClient GetInstance(string envVar, EnvironmentVariableTarget target);
    }
}
