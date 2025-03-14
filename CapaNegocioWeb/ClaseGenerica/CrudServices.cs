﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace CapaNegocioWeb.ClaseGenerica
{
    public class CrudServices<T> where T : class
    {
        /// <summary>
        /// Conexion a la BD
        /// </summary>
        private readonly DbContext db;

        public CrudServices(DbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Funcion para listar todos los registros de una entidad
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            return db.Set<T>().ToList();
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return db.Set<T>().Where(expression).ToList();
        }

        /// <summary>
        /// Funcion para crear una registro de una entidad
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool Crear(T entidad)
        {
            try
            {
                Console.WriteLine($"Entidad recibida: {Newtonsoft.Json.JsonConvert.SerializeObject(entidad)}");
                db.Entry(entidad).State = EntityState.Added;
                int resultado = db.SaveChanges();
                Console.WriteLine($"Filas afectadas: {resultado}");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la entidad: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Funcion para actualizar un registro de una entidad
        /// </summary>
        /// <param name="entidad"></param>
        public bool Actualizar(T entidad)
        {
            try
            {
                // Marcar la entidad como modificada
                db.Entry(entidad).State = EntityState.Modified;

                // Guardar los cambios en la base de datos
                db.SaveChanges();

                // Si todo va bien, devolver true (actualización exitosa)
                return true;
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes capturar el error específico de actualización de la base de datos
                // y devolver false si ocurre un problema
                throw new Exception("Error al actualizar la entidad en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                // Captura cualquier otro tipo de error inesperado
                // y devuelve false en caso de un error general
                throw new Exception("Error inesperado al actualizar la entidad.", ex);
            }
        }

        /// <summary>
        /// Funicon para eliminar un registro de una entidad
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Eliminar(int id)
        {
            var entidad = db.Set<T>().Find(id);

            db.Entry(entidad).State = EntityState.Deleted;

            return db.SaveChanges() > 0;
        }

        /// <summary>
        /// Funicon para eliminar un registro de una entidad
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T BuscarId(int id)
        {
            var entidad = db.Set<T>().Find(id);
            return entidad;
        }
    }
}