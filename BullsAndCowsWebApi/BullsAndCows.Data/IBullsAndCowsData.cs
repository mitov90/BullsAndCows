namespace BullsAndCows.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using BullsAndCows.Data.Repositories;
    using BullsAndCows.Models;

    public interface IBullsAndCowsData
    {
        IRepository<Player> Players { get; }
        
        IRepository<Game> Games { get; }

        IRepository<Notification> Notifications { get; }

        IRepository<Guess> Guesses { get; }

        int SaveChanges();
    }
}