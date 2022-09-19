using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        // INITIAL DATABASE
        private ApplicationContext _context;
        // INITIAL REPOSITORIES
        private IRepository<Platform> _platformRepository;
        private IRepository<Status> _statusRepository;
        private IRepository<Serial> _serialRepository;
        private IRepository<Movie> _movieRepository;
        private IRepository<PlatformMovie> _platformMovieRepository;
        private IRepository<PlatformSerial> _platformSerialRepository;
        public UnitOfWork(ApplicationContext context) { _context = context; } // CTOR
        // GET FOR REPOSITORY
        public IRepository<Platform> PlatformRepository
        {
            get
            {
                if (_platformRepository == null)
                    _platformRepository = new Repository<Platform>(_context);
                return _platformRepository;
            }
        }
        public IRepository<PlatformMovie> PlatformMovieRepository
        {
            get
            {
                if (_platformMovieRepository == null)
                    _platformMovieRepository = new Repository<PlatformMovie>(_context);
                return _platformMovieRepository;
            }
        }
        public IRepository<PlatformSerial> PlatformSerialRepository
        {
            get
            {
                if (_platformSerialRepository == null)
                    _platformSerialRepository = new Repository<PlatformSerial>(_context);
                return _platformSerialRepository;
            }
        }
        public IRepository<Status> StatusRepository
        {
            get
            {
                if (_statusRepository == null)
                    _statusRepository = new Repository<Status>(_context);
                return _statusRepository;
            }
        }
        public IRepository<Movie> MovieRepository
        {
            get
            {
                if (_movieRepository == null)
                    _movieRepository = new Repository<Movie>(_context);
                return _movieRepository;
            }
        }
        public IRepository<Serial> SerialRepository
        {
            get
            {
                if (_serialRepository == null)
                    _serialRepository = new Repository<Serial>(_context);
                return _serialRepository;
            }
        }
        // REALISE Save();
        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
        // DISPOSING
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    _context.Dispose();
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
