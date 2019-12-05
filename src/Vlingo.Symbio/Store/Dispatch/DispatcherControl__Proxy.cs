using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vlingo.Actors;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.Dispatch
{
    public class DispatcherControl__Proxy : Vlingo.Symbio.Store.Dispatch.IDispatcherControl
    {
        private const string ConfirmDispatchedRepresentation1 =
            "ConfirmDispatched(string, Vlingo.Symbio.Store.Dispatch.IConfirmDispatchedResultInterest)";

        private const string DispatchUnconfirmedRepresentation2 = "DispatchUnconfirmed()";
        private const string StopRepresentation3 = "Stop()";

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public DispatcherControl__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public void ConfirmDispatched(string dispatchId,
            Vlingo.Symbio.Store.Dispatch.IConfirmDispatchedResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Dispatch.IDispatcherControl> cons128873 = __ =>
                    __.ConfirmDispatched(dispatchId, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, ConfirmDispatchedRepresentation1);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Dispatch.IDispatcherControl>(this.actor,
                        cons128873, ConfirmDispatchedRepresentation1));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, ConfirmDispatchedRepresentation1));
            }
        }

        public void DispatchUnconfirmed()
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Dispatch.IDispatcherControl> cons128873 = __ => __.DispatchUnconfirmed();
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, DispatchUnconfirmedRepresentation2);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Dispatch.IDispatcherControl>(this.actor,
                        cons128873, DispatchUnconfirmedRepresentation2));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, DispatchUnconfirmedRepresentation2));
            }
        }

        public void Stop()
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Dispatch.IDispatcherControl> cons128873 = __ => __.Stop();
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, StopRepresentation3);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Dispatch.IDispatcherControl>(this.actor, cons128873,
                            StopRepresentation3));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, StopRepresentation3));
            }
        }
    }
}