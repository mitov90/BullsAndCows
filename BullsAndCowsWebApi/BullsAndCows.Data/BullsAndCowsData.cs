﻿namespace BullsAndCows.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using BullsAndCows.Data.Repositories;
    using BullsAndCows.Models;

    public class BullsAndCowsData : IBullsAndCowsData 
    {
        private DbContext context;
        private IDictionary<Type, object> repositories;

        public BullsAndCowsData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<Player> Players
        {
            get
            {
                return this.GetRepository<Player>();
            }
        }

        public IRepository<Game> Games
        {
            get
            {
                return this.GetRepository<Game>();
            }
        }

        public IRepository<Notification> Notifications
        {
            get
            {
                return this.GetRepository<Notification>();
            }
        }

        public IRepository<Guess> Guesses
        
        {
            get
            {
                return this.GetRepository<Guess>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);
            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository = Activator.CreateInstance(typeof(EFRepository<T>), context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}