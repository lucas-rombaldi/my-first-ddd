using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDDDStore.Core.Communication.Mediator;
using MyDDDStore.Core.Messages.CommomMessages.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyDDDStore.WebApp.MVC.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatrHandler _mediatorHandler;

        protected Guid ClienteId = Guid.Parse("918811AC-3B41-4143-8E94-E8F955986F13");

        public ControllerBase(INotificationHandler<DomainNotification> notifications, IMediatrHandler mediatorHandler)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler;
        }

        protected bool ValidOperation()
        {
            return !_notifications.HasNotification();
        }        

        protected void NotifyError(string codigo, string mensagem)
        {
            _mediatorHandler.PublishNotification(new DomainNotification(codigo, mensagem));
        }

        protected IEnumerable<string> GetErrorMessages()
        {
            return _notifications.GetNotifications().Select(s => s.Value).ToList();
        }
    }
}
