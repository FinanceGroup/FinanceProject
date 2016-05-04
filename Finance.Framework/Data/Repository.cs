﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Metadata;
using NHibernate.Type;
using Finance.Framework.Logging;
using Finance.Framework.Utility.Extensions;

namespace Finance.Framework.Data {
    public class Repository<T> : IRepository<T> where T : class {
        private readonly ISessionLocator _sessionLocator;

        public Repository(ISessionLocator sessionLocator/*, ShellSettings shellSettings*/) {
            _sessionLocator = sessionLocator;
            //Shellsettings = shellSettings;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        //public ShellSettings Shellsettings { get; set; }

        protected virtual ISessionLocator SessionLocator {
            get { return _sessionLocator; }
        }

        public virtual ISession Session {
            get { return SessionLocator.For(typeof (T)); }
        }

        public virtual IQueryable<T> Table {
            get {
               var result = Session.Query<T>().Cacheable();
               //if (typeof(IPropertyAware).IsAssignableFrom(typeof(T))) {
               //     result = result.Where(x => ((IPropertyAware)x).CompanyCode == Shellsettings.Company 
               //         && ((IPropertyAware)x).PropertyCode == Shellsettings.Property);
               //}

                return result;
            }
        }

        #region IRepository<T> Members

        void IRepository<T>.Create(T entity) {
            Create(entity);
        }

        void IRepository<T>.Update(T entity) {
            Update(entity);
        }

        void IRepository<T>.Delete(T entity) {
            Delete(entity);
        }

        void IRepository<T>.Copy(T source, T target) {
            Copy(source, target);
        }

        void IRepository<T>.Flush() {
            Flush();
        }

        T IRepository<T>.Get(int id) {
            return Get(id);
        }

        T IRepository<T>.Get(long id) {
            return Get(id);
        }

        T IRepository<T>.Get(string id) {
            return Get(id);
        }

        T IRepository<T>.Get(Expression<Func<T, bool>> predicate) {
            return Get(predicate);
        }

        IQueryable<T> IRepository<T>.Table {
            get { return Table; }
        }

        int IRepository<T>.Count(Expression<Func<T, bool>> predicate) {
            return Count(predicate);
        }

        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate) {
            return Fetch(predicate).ToReadOnlyCollection();
        }

        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order) {
            return Fetch(predicate, order).ToReadOnlyCollection();
        }

        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip,
                                            int count) {
            return Fetch(predicate, order, skip, count).ToReadOnlyCollection();
        }

        #endregion

        public virtual T Get(int id) {
            return Session.Get<T>(id);
        }
        
        public virtual T Get(long id) {
            return Session.Get<T>(id);
        }

        public virtual T Get(string id) {
            return Session.Get<T>(id);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate) {
            return Fetch(predicate).SingleOrDefault();
        }

        public virtual void Create(T entity) {
            Logger.Debug("Create {0}", entity);

            //if (entity is IPropertyAware){
            //    var propertyAwareEntity = entity as IPropertyAware;

            //    if (null != propertyAwareEntity
            //        && string.IsNullOrEmpty(propertyAwareEntity.CompanyCode)
            //        && string.IsNullOrEmpty(propertyAwareEntity.PropertyCode)) {

            //        propertyAwareEntity.CompanyCode = Shellsettings.Company;
            //        propertyAwareEntity.PropertyCode = Shellsettings.Property;
            //    }
            //}

            Session.Save(entity);
            Session.Flush();
        }

        public virtual void Update(T entity) {
            Logger.Debug("Update {0}", entity);
            Session.Evict(entity);
            Session.Merge(entity);
            Session.Flush();
        }

        public virtual void Delete(T entity) {
            Logger.Debug("Delete {0}", entity);
            Session.Delete(entity);
            Session.Flush();
        }

        public virtual void Copy(T source, T target) {
            Logger.Debug("Copy {0} {1}", source, target);
            var metadata = Session.SessionFactory.GetClassMetadata(typeof (T));
            var values = metadata.GetPropertyValues(source, EntityMode.Poco);

            for (int i = 0; i < metadata.PropertyTypes.Length; i++) {
                if (metadata.PropertyTypes[i].IsCollectionType) {
                    values[i] = Copy(metadata.PropertyTypes[i] as CollectionType, values[i] as IEnumerable);
                }
            }

            metadata.SetPropertyValues(target, values, EntityMode.Poco);
        }

        public virtual void Flush() {
            Session.Flush();
        }

        public virtual int Count(Expression<Func<T, bool>> predicate) {
            return Fetch(predicate).Count();
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate) {
            return Table.Where(predicate);
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order) {
            var orderable = new Orderable<T>(Fetch(predicate));
            order(orderable);
            return orderable.Queryable;
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip,
                                           int count) {
            return Fetch(predicate, order).Skip(skip).Take(count);
        }

        private object Copy(CollectionType collectionType, IEnumerable enumerable) {
            IList result = collectionType.Instantiate(-1) as IList;

            IClassMetadata metadata = null;

            foreach (var item in enumerable) {
                if (null == metadata) {
                    metadata = Session.SessionFactory.GetClassMetadata(item.GetType());
                }

                var target = Activator.CreateInstance(item.GetType());
                var values = metadata.GetPropertyValues(item, EntityMode.Poco);

                for (int i = 0; i < metadata.PropertyTypes.Length; i++) {
                    if (metadata.PropertyTypes[i].IsCollectionType) {
                        values[i] = Copy(metadata.PropertyTypes[i] as CollectionType, values[i] as IEnumerable);
                    }
                }

                metadata.SetPropertyValues(target, values, EntityMode.Poco);

                result.Add(target);
            }

            return result;
        }
    }
}