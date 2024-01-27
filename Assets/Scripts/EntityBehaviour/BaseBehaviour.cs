using UnityEngine;

namespace TUF.EntityBehaviour
{
    /// <summary>
    /// Classe astratta che definisce un comportamento base
    /// </summary>
    public abstract class BaseBehaviour : MonoBehaviour, IBehaviour
    {
        /// <summary>
        /// Riferimento all'entitià che controlla il Behaviour
        /// </summary>
        protected IEntity entity;
        /// <summary>
        /// Riferimento al dato dell'entità che controlla il Behaviour
        /// </summary>
        protected IEntityData entityData;
        /// <summary>
        /// True se il Behaviour è stato setuppato, false altrimenti
        /// </summary>
        protected bool isSetupped;
        /// <summary>
        /// True se il Behaviour è attivo, false altrimenti
        /// </summary>
        protected bool isActive;

        /// <summary>
        /// Funzione di setup del behaviour
        /// </summary>
        /// <param name="_entity"></param>
        public virtual void Setup(IEntity _entity)
        {
            entity = _entity;
            isSetupped = true;
        }
        /// <summary>
        /// Funzione di setup del behaviour
        /// </summary>
        /// <param name="_entity"></param>
        /// <param name="_entityData"></param>
        public virtual void Setup(IEntity _entity, IEntityData _entityData)
        {
            entity = _entity;
            entityData = _entityData;
            isSetupped = true;
        }
        /// <summary>
        /// Funzione di unsetp del behaviour
        /// </summary>
        public virtual void Unsetup()
        {
            isSetupped = false;
        }
        /// <summary>
        /// True se il Behaviour è stato setuppato, false altrimenti
        /// </summary>
        public bool IsSetupped()
        {
            return isSetupped;
        }

        /// <summary>
        /// Behaviour's custom update.
        /// </summary>
        public virtual void OnFixedUpdate() { }
        /// <summary>
        /// Behaviour's custom fixed update.
        /// </summary>
        public virtual void OnLateUpdate() { }
        /// <summary>
        /// Behaviour's custom late update.
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// Funzione che si occupa di attivare il Behaviour
        /// </summary>
        public virtual void Enable()
        {
            isActive = true;
        }
        /// <summary>
        /// Funzione che si occupa di disattivare il Behaviour
        /// </summary>
        public virtual void Disable()
        {
            isActive = false;
        }
        /// <summary>
        /// True se il Behaviour è attivo, false altrimenti
        /// </summary>
        public bool IsActive()
        {
            return isActive;
        }
    }
}