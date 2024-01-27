using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TUF.EntityBehaviour
{
    /// <summary>
    /// Classe astratta che definisce un'entità base
    /// </summary>
    public abstract class BaseEntity : MonoBehaviour, IEntity
    {
        /// <summary>
        /// Lista di behaviour dell'entità
        /// </summary>
        protected List<IBehaviour> behaviours;
        /// <summary>
        /// Riferimento al dato dell'entità
        /// </summary>
        protected IEntityData entityData;
        /// <summary>
        /// True se l'entità è setuppata, false altrimenti
        /// </summary>
        protected bool isSetupped;
        /// <summary>
        /// True se l'entità è attiva, false altrimenti
        /// </summary>
        protected bool isActive;

        /// <summary>
        /// Funzione di setup dell'entità
        /// </summary>
        public virtual void Setup()
        {
            behaviours = GetComponentsInChildren<IBehaviour>().ToList();

            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].Setup(this);

            isSetupped = true;
        }
        /// <summary>
        /// Funzione di setup dell'entità
        /// </summary>
        /// <param name="_entityData"></param>
        public virtual void Setup(IEntityData _entityData)
        {
            behaviours = GetComponentsInChildren<IBehaviour>().ToList();
            entityData = _entityData;

            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].Setup(this, _entityData);

            isSetupped = true;
        }
        /// <summary>
        /// Unsetp dell'entità
        /// </summary>
        public virtual void Unsetup()
        {
            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].Unsetup();

            isSetupped = false;
        }
        /// <summary>
        /// True se l'entità è stato setuppata, false altrimenti
        /// </summary>
        public bool IsSetupped()
        {
            return isSetupped;
        }

        /// <summary>
        /// Funzione che ritorna il behaviour corrispondente al tipo passato
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetBehaviour<T>() where T : IBehaviour
        {
            for (int i = 0; i < behaviours.Count; i++)
                if (behaviours[i].GetType() == (typeof(T)))
                    return (T)behaviours[i];

            return default(T);
        }
        /// <summary>
        /// Funzione che ritorna il GameObject dell'entità
        /// </summary>
        /// <returns></returns>
        public GameObject GetGameObject()
        {
            return gameObject;
        }

        /// <summary>
        /// Funzione che si occupa di attivare l'entità
        /// </summary>
        public virtual void Enable()
        {
            isActive = true;

            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].Enable();
        }
        /// <summary>
        /// Funzione che si occupa di disattivare l'entità
        /// </summary>
        public virtual void Disable()
        {
            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].Disable();

            isActive = false;
        }
        /// <summary>
        /// True se l'entità è attiva, false altrimenti
        /// </summary>
        public bool IsActive()
        {
            return isActive;
        }

        /// <summary>
        /// Funzione che ritorna lo stato dell'enità
        /// </summary>
        /// <returns></returns>
        public bool IsSetuppedAndActive()
        {
            return isSetupped && isActive;
        }

        #region Updates
        /// <summary>
        /// Behaviour's custom late update.
        /// </summary>
        public virtual void OnUpdate()
        {
            if (!isSetupped)
                return;

            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].OnUpdate();
        }
        /// <summary>
        /// Behaviour's custom update.
        /// </summary>
        public virtual void OnFixedUpdate()
        {
            if (!isSetupped)
                return;

            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].OnFixedUpdate();
        }
        /// <summary>
        /// Behaviour's custom fixed update.
        /// </summary>
        public virtual void OnLateUpdate()
        {
            if (!isSetupped)
                return;

            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].OnLateUpdate();
        }

        /// <summary>
        /// Unity Update
        /// </summary>
        protected void Update()
        {
            OnUpdate();
        }
        /// <summary>
        /// Unity FixedUpdate
        /// </summary>
        protected void FixedUpdate()
        {
            OnFixedUpdate();
        }
        /// <summary>
        /// Unity LateUpdate
        /// </summary>
        protected void LateUpdate()
        {
            OnLateUpdate();
        }
        #endregion
    } 
}