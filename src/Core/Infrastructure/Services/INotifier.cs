using Domain.AggregatesModel.InstrumentsAggregate;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public interface INotifier
    {
        void SendMail(string subject, string body);
        void SendMail(string subject, string body, IEnumerable<Instrument> instrumentDomains);
    }
}
