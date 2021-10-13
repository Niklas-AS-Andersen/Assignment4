using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using Assignment4.Core;
using static Assignment4.Core.Response;

namespace Assignment4.Entities
{
    public class TagRepository: ITagRepository
    {
        private readonly KanbanContext _context;

        public TagRepository(KanbanContext context)
        {
            _context = context;
        }
        public (Response Response, int TagId) Create(TagCreateDTO tag)
        {
            if (_context.Tags.Find(tag.Name) != null) return (Conflict, -1);

            var t = new Tag{
                Name = tag.Name,
            };

            _context.Tags.Add(t);
            _context.SaveChanges();

            return (Created, t.Id);
        }

        public IReadOnlyCollection<TagDTO> ReadAll() =>
            _context.Tags
                .Select(t => new TagDTO(t.Id, t.Name)).ToList().AsReadOnly();

        public TagDTO Read(int tagId) =>
            _context.Tags
                .Select(t => new TagDTO(t.Id, t.Name))
                .Where(t => t.Id == tagId).FirstOrDefault();

        public Response Update(TagUpdateDTO tag)
        {
            var entity = _context.Tags.Find(tag.Id);

            if (entity == null) return NotFound;

            if (_context.Tags.Find(tag.Name) != null) return Conflict;

            entity.Name = tag.Name;

            _context.SaveChanges();

            return Updated;
        }

        public Response Delete(int tagId, bool force = false)
        {
            if (!force) return Conflict;

            var entity = _context.Tags.Find(tagId);
            _context.Tags.Remove(entity);

            _context.SaveChanges();

            return Deleted;
        }
    }
}
