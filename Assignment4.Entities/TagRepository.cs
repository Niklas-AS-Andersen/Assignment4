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
        public (Response Response, int TagId) Create(TagCreateDTO tag)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TagDTO> ReadAll()
        {
            throw new NotImplementedException();
        }

        public TagDTO Read(int tagId)
        {
            throw new NotImplementedException();
        }

        public Response Update(TagUpdateDTO tag)
        {
            throw new NotImplementedException();
        }

        public Response Delete(int tagId, bool force = false)
        {
            throw new NotImplementedException();
        }
    }
}
