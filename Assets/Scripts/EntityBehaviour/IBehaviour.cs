
namespace TUF.EntityBehaviour
{
    /// <summary>
    /// Interfaccia che definisce un Behaviour
    /// </summary>
    public interface IBehaviour
    {
        /// <summary>
        /// Funzione di setup del Behaviour
        /// </summary>
        /// <param name="_entity">Riferimento all'entità che gestisce il Behaviour</param>
        void Setup(IEntity _entity);
        /// <summary>
        /// Funzione di setup del Behaviour
        /// </summary>
        /// <param name="_entity">Riferimento all'entità che gestisce il Behaviour</param>
        /// <param name="_entityData">Riferimento al dato dell'entità che gestisce il Behaviour</param>
        void Setup(IEntity _entity, IEntityData _entityData);
        /// <summary>
        /// Funzione di unsetp del behaviour
        /// </summary>
        void Unsetup();
        /// <summary>
        /// True se il Behaviour è stato setuppato, false altrimenti
        /// </summary>
        bool IsSetupped();

        /// <summary>
        /// Update del Behaviour
        /// </summary>
        void OnUpdate();
        /// <summary>
        /// FixedUpdate del Behaviour
        /// </summary>
        void OnFixedUpdate();
        /// <summary>
        /// LateUpdate del Behaviour
        /// </summary>
        void OnLateUpdate();

        /// <summary>
        /// Funzione che si occupa di attivare il Behaviour
        /// </summary>
        void Enable();
        /// <summary>
        /// Funzione che si occupa di disattivare il Behaviour
        /// </summary>
        void Disable();
        /// <summary>
        /// True se il Behaviour è attivo, false altrimenti
        /// </summary>
        bool IsActive();
    }
}
