using System.Collections.ObjectModel;
using System.Data.Entity;
using WpfMailSender.Model;

namespace WpfMailSender.Services
{
    public class DataAccessService : IDataAccessService
    {
        EmailsEntities context;

        public DataAccessService()
        {
            context = new EmailsEntities();
        }

        /// <summary>
        /// Возвращает список получателей
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Email> GetEmails()
        {
            ObservableCollection<Email> Emails = new ObservableCollection<Email>();
            foreach (Email item in context.EmailSet)
            {
                Emails.Add(item);
            }
            return Emails;
        }

        /// <summary>
        /// Возвращает список серверов для отправки
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Server> GetServers()
        {
            ObservableCollection<Server> servers = new ObservableCollection<Server>();
            foreach (var item in context.ServerSet)
            {
                servers.Add(item);
            }
            return servers;
        }

        public int AddEmail(Email email)
        {
            context.EmailSet.Add(email);
            context.SaveChanges();
            return email.Id;
        }

        public int UpdateEmail(Email email)
        {
            context.EmailSet.Attach(email);
            context.Entry(email).State = EntityState.Modified;
            context.SaveChanges();
            return email.Id;
        }

        public int DeleteEmail(Email email)
        {
            if (context.Entry(email).State == EntityState.Detached)
            {
                context.EmailSet.Attach(email);
            }
            context.EmailSet.Remove(email);

            context.SaveChanges();
            return email.Id;
        }
    }
}
