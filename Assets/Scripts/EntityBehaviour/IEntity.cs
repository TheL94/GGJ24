using UnityEngine;

namespace TUF.EntityBehaviour
{
    /// <summary>
    /// Interfaccia che definisce il comportamento di un Entity
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Funzione di setup dell'entità
        /// </summary>
        void Setup();
        /// <summary>
        /// Funzione di setup dell'entità
        /// </summary>
        /// <param name="_entityData">Dato dell'entità</param>
        void Setup(IEntityData _entityData);
        /// <summary>
        /// Unsetp dell'entità
        /// </summary>
        void Unsetup();
        /// <summary>
        /// True se l'entità è stato setuppata, false altrimenti
        /// </summary>
        bool IsSetupped();

        /// <summary>
        /// Funzione che ritorna il behaviour corrispondente al tipo passato
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetBehaviour<T>() where T : IBehaviour;
        /// <summary>
        /// Funzione che ritorna il GameObject dell'entità
        /// </summary>
        /// <returns></returns>
        GameObject GetGameObject();

        /// <summary>
        /// Update dell'entità
        /// </summary>
        void OnUpdate();
        /// <summary>
        /// FixedUpdate dell'entità
        /// </summary>
        void OnFixedUpdate();
        /// <summary>
        /// LateUpdate dell'entità
        /// </summary>
        void OnLateUpdate();

        /// <summary>
        /// Funzione che si occupa di attivare l'entità
        /// </summary>
        void Enable();
        /// <summary>
        /// Funzione che si occupa di disattivare l'entità
        /// </summary>
        void Disable();
        /// <summary>
        /// True se l'entità è attiva, false altrimenti
        /// </summary>
        bool IsActive();
    }

    /// <summary>
    /// Data needed for the entity to work, that comes from outside the entity's behaviours or the entity itself.
    /// </summary>
    public interface IEntityData { }
}