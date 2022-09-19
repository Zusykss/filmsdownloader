using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Movie> MovieRepository { get; }
        IRepository<Platform> PlatformRepository { get; }
        IRepository<Serial> SerialRepository { get; }
        IRepository<Status> StatusRepository { get; }
        IRepository<PlatformMovie> PlatformMovieRepository { get; }
        IRepository<PlatformSerial> PlatformSerialRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
