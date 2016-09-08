﻿using Abp.Domain.Services;
using CInotam.MailSender.Contracts;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.MailSender.CinotamMailSender
{
    public interface ICinotamMailSender : IMailServiceProvider, IDomainService
    {
    }
}
