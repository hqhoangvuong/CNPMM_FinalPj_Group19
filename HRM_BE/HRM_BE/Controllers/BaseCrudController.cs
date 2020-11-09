using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRM.Core.Data;
using HRM.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRM.API.Controllers
{
    public abstract class BaseCrudViewModelController<T, TViewModel, TKey> : ControllerBase where T : BaseEntity<TKey>
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;

        protected BaseCrudViewModelController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> List(CancellationToken token)
        {
            var entities = await _context.Set<T>().ProjectTo<TViewModel>(_mapper.ConfigurationProvider).ToListAsync(token);
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] TKey id, CancellationToken token)
        {
            var entity = await _context.Set<T>().FindAsync(new object[] { id }, token);
            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<T, TViewModel>(entity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] TKey id, [FromBody] T entity)
        {
            if (!id.Equals(entity.Id))
                return BadRequest();

            var original = await _context.Set<T>().FindAsync(id);
            if (original == null)
                return NotFound();

            _context.Entry(original).State = EntityState.Detached;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] TKey id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.Entry(entity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public abstract class BaseCrudViewModelController<T, TViewModel> : BaseCrudViewModelController<T, TViewModel, int> where T : BaseEntity
    {
        protected BaseCrudViewModelController(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }
    }

    public abstract class BaseCrudController<T, TKey> : ControllerBase where T : BaseEntity<TKey>
    {
        private readonly ApplicationDbContext _context;

        protected BaseCrudController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> List(CancellationToken token)
        {
            var entities = await _context.Set<T>().ToListAsync(token);
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] TKey id, CancellationToken token)
        {
            var entity = await _context.Set<T>().FindAsync(new object[] { id }, token);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] TKey id, [FromBody] T entity)
        {
            if (!id.Equals(entity.Id))
                return BadRequest();

            var original = await _context.Set<T>().FindAsync(id);
            if (original == null)
                return NotFound();

            _context.Entry(original).State = EntityState.Detached;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] TKey id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.Entry(entity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public abstract class BaseCrudController<T> : BaseCrudController<T, int> where T : BaseEntity
    {
        protected BaseCrudController(ApplicationDbContext context) : base(context) { }
    }
}
