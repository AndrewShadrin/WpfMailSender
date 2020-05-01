using System.Collections.ObjectModel;
using WpfMailSender.Model;

namespace WpfMailSender.Services
{
    public class DataAccessService : IDataAccessService
    {
        EmailsDataContext context;

        public DataAccessService()
        {
            context = new EmailsDataContext();
        }

        /// <summary>
        /// Возвращает список получателей
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Email> GetEmails()
        {
            ObservableCollection<Email> Emails = new ObservableCollection<Email>();
            foreach (var item in context.Email)
            {
                Emails.Add(item);
            }
            return Emails;
        }

        /// <summary>
        /// Возвращает список серверов для отправки
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Servers> GetServers()
        {
            ObservableCollection<Servers> servers = new ObservableCollection<Servers>();
            foreach (var item in context.Servers)
            {
                servers.Add(item);
            }
            return servers;
        }

        public int CreateEmail(Email email)
        {
            context.Email.InsertOnSubmit(email);
            context.SubmitChanges();
            return email.Id;
        }
    }
}
