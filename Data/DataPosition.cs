using Company.Data.AvtoGenerate.Entity;
using Company.Data.dataContext;
using Company.Dtos;
using Company.Dtos.FilterDto;
using Company.Repository.Positions;
using Company.Repository.Positions.Models;

namespace Company.Data;


public class DataPosition : IPositionRepository
{
    private readonly DbContextdta _context;

    public DataPosition(DbContextdta context) 
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public int Create(PositionCreateDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var position = new Position
        {
            PositionName = dto.PositionName,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Positions.Add(position);
        _context.SaveChanges();

        return position.PositionId;
    }
    public bool Update(PositionUpdate dto, int id)
    {
        if(dto == null) throw new NotImplementedException(nameof(dto));

        var position = _context.Positions.Find(id);
        if ( position == null)
            return false;

        position.PositionName = dto.PositionName;
        position.Description = dto.Description;
        position.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var position = _context.Positions.Find(id);
        if ( position == null) return false;

        _context.Positions.Remove(position);
        _context.SaveChanges();
        return true;
    }

    public PositionDto GetById(int id)
    {
        var position = _context.Positions.Find(id);
        if (position == null) return null;

        return new PositionDto
        {
            PositionId = position.PositionId,
            PositionName = position.PositionName,
            Description = position.Description,
            CreatedAt = position.CreatedAt,
            UpdatedAt = position.UpdatedAt,
        };
    }

    public List<PositionDto> GetAll( PositionFilterDto filter )
    {
        var query = _context.Positions
            .SortFilter( filter )
            .AsQueryable();

       return query.Select(s => new PositionDto 
       {
           PositionId = s.PositionId,
           PositionName = s.PositionName,
           Description = s.Description,
           CreatedAt = s.CreatedAt,
           UpdatedAt = s.UpdatedAt,
       }).ToList();
    }
}
