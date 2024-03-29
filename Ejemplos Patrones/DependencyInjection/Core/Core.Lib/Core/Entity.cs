﻿using Common.Lib.Core.Context;
using Common.Lib.Infrastructure;
using System;

namespace Common.Lib.Core
{
    public abstract class  Entity
    {
        #region Statics

        public static IDependencyContainer DepCon { get; set; }

        #endregion

        public Guid Id { get; set; }

        public ValidationResult CurrentValidation { get; private set; }

        public virtual SaveResult<T> Save<T>() where T : Entity
        {
            var output = new SaveResult<T>();

            CurrentValidation = Validate();

            if (CurrentValidation.IsSuccess)
            {
                var repo = DepCon.Resolve<IRepository<T>>();

                if (this.Id == Guid.Empty)
                {
                    this.Id = Guid.NewGuid();
                    output = repo.Add(this as T);
                }
                else
                {
                    output = repo.Update(this as T);
                }
            }

            output.Validation = CurrentValidation;

            return output;
        }

        public virtual ValidationResult Validate()
        {
            var output = new ValidationResult()
            {
                IsSuccess = true
            };

            return output;
        }
    }
}
