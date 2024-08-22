using Application.DTO.Application.Motorcycle;
using Confluent.Kafka;

namespace Application.Interfaces
{
    public interface IMotorcycleAdapterService : IMessageAdapter<MotorcycleAddRequestDTO>
    {
        Task<PersistenceStatus> SendAsync(MotorcycleAddRequestDTO request);
    }
}
