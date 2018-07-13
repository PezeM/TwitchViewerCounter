﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchViewerCounter.Database.Entities;

namespace TwitchViewerCounter.Database.Repositories
{
    public abstract class BaseMongoRepository<TEntity> : IRepository<TEntity, string> where TEntity : IEntity
    {
        protected abstract IMongoCollection<TEntity> Collection { get; }

        public virtual async Task<TEntity> GetByIdAsync(string id)
        {
            return await Collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> SaveAsync(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                entity.Id = ObjectId.GenerateNewId().ToString();
            }

            await Collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity,
                new UpdateOptions
                {
                    IsUpsert = true
                });

            return entity;
        }

        public virtual async Task DeleteAsync(string id)
        {
            await Collection.DeleteOneAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<ICollection<TEntity>> FindAllAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }
    }
}
