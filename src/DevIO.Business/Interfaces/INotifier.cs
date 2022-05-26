using System.Collections.Generic;
using DevIO.Business.Notifications;

namespace DevIO.Business.Interfaces
{
	public interface INotifier
	{
		public bool HasNotification();

		public List<Notification> GetNotifications();

		public void Handle(Notification notifcation);
	}
}

