using System.Linq.Expressions;
using Dal.Contracts;
using Dal.Data;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public class RelationshipRepository : Repository<Relationship>,IRelationshipRepository
{
    public RelationshipRepository(PersonDbContext context) : base(context) 
    {
        
    }

    public async Task AddRelationshipAsync(Relationship relationship)
    {
        await CreateAsync(relationship);
    }

    public async Task DeleteRelationshipAsync(int relationshipId)
    {
        var relationship = await DbSet.FindAsync(relationshipId);
        if (relationship != null)
        {
            DbSet.Remove(relationship);
        }
    }
    public async Task UpdateRelationshipAsync(Relationship relationship)
    {
        DbSet.Update(relationship);
    }

    public async Task<IEnumerable<Relationship>> GetPersonRelationshipsAsync(int personId)
    {
        return await DbSet
            .Include(r => r.Person)
            .Include(r => r.RelatedPerson)
            .Where(r => r.PersonId == personId || r.RelatedId == personId)
            .ToListAsync();
    }
}