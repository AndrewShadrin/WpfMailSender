﻿using System.Collections.ObjectModel;
using WpfMailSender.Model;

namespace WpfMailSender.Services
{
    public interface IDataAccessService
    {
        /// <summary>
        /// Возвращает список получателей
        /// </summary>
        /// <returns></returns>
        ObservableCollection<Email> GetEmails();
        
        /// <summary>
        /// Возвращает список серверов для отправки
        /// </summary>
        /// <returns></returns>
        ObservableCollection<Server> GetServers();

        int AddEmail(Email email);

        int UpdateEmail(Email email);

        int DeleteEmail(Email email);
    }
}
