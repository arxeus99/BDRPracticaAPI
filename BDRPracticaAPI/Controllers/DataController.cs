using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDRPracticaAPI.Controllers
{
    public class DataController : Controller
    {

        DataBaseHelperDataContext DataBaseHelper;

        public DataController()
        {
            DataBaseHelper = new DataBaseHelperDataContext();
        }


        #region Paises
        public JsonResult GetPaises()
        {

            var result = Json("");

            try
            {
                var paises = from p in DataBaseHelper.Paises select p;

                result = Json(paises);

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }
            catch (Exception ex)
            {
                result = Json($"Error: {ex.Message}");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }

        }

        public JsonResult GetPais(Int64 IdPais)
        {
            var result = Json("");

            try
            {
                var paisSelect = from p in DataBaseHelper.Paises where p.Id == IdPais select p;

                if (paisSelect.Count() == 0)
                {
                    result = Json("Error: No se ha encontrado pais para la Id indicada");

                    result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                    return result;
                }

                var pais = paisSelect.First();

                result = Json(pais);

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }
            catch (Exception ex)
            {
                result = Json($"Error: {ex.Message}");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }
        }

        [HttpPost]
        public JsonResult InsertPais(Paise pais)
        {

            var result = Json("");

            try
            {
                DataBaseHelper.Paises.InsertOnSubmit(pais);

                DataBaseHelper.SubmitChanges();

                result = Json("Pais insertado correctamente");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


                return result;
            }
            catch (Exception ex)
            {
                result = Json($"Error: {ex.Message}");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }

        }

        public JsonResult DeletePais(Int64 IdPais)
        {
            var result = Json("");

            try
            {
                var paisSelect = from p in DataBaseHelper.Paises where p.Id == IdPais select p;

                if(paisSelect.Count() == 0)
                {
                    result = Json("Error: No se ha encontrado pais para la Id indicada");

                    result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                    return result;
                }

                var pais = paisSelect.First();

                DataBaseHelper.Paises.DeleteOnSubmit(pais);

                DataBaseHelper.SubmitChanges();

                result = Json("Pais eliminado con exito");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("FK_Clientes_ToPaises"))
                    result = Json("Error: No se puede eliminar el pais porque está asignado a uno o varios clientes. Eimine antes los clientes");
                else
                    result = Json($"Error: {ex.Message}");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }
        }

        [HttpPost]
        public JsonResult UpdatePais(Paise newPais)
        {

            var result = Json("");

            try
            {
                var oldPaisSelect = (from p in DataBaseHelper.Paises where p.Id == newPais.Id select p);

                if (oldPaisSelect.Count() == 0)
                {
                    result = Json("Error: No se ha encontrado pais para la Id indicada");

                    result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                    return result;
                }

                var oldPais = oldPaisSelect.First();

                oldPais.Nombre = newPais.Nombre;

                DataBaseHelper.SubmitChanges();

                result = Json("Pais actualizado correctamente");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;

            }
            catch (Exception ex)
            {

                result = Json($"Error: {ex.Message}");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }

        }

        #endregion


        #region Clientes

        public JsonResult GetClientes()
        {

            var result = Json("");

            try
            {
                var clientes = from c in DataBaseHelper.Clientes
                               join p in DataBaseHelper.Paises on c.IdPais equals p.Id
                               select new
                               {
                                   Id = c.Id,
                                   Nombre = c.Nombre,
                                   Direccion = c.Direccion,
                                   Poblacion = c.Poblacion,
                                   IdPais = c.IdPais,
                                   NombrePais = p.Nombre,
                                   Telefono = c.Telefono,
                                   Email = c.Email
                               };

                result = Json(clientes);

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }
            catch (Exception ex)
            {
                result = Json($"Error: {ex.Message}");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }

        }



        [HttpPost]
        public JsonResult InsertCliente(Cliente cliente)
        {
            var result = Json("");

            try
            {
                var paisSelect = (from p in DataBaseHelper.Paises where p.Id == cliente.IdPais select p);

                if (paisSelect.Count() == 0)
                {
                    result = Json("Error: No se ha encontrado pais para la Id indicada a este cliente");

                    result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                    return result;
                }

                DataBaseHelper.Clientes.InsertOnSubmit(cliente);

                DataBaseHelper.SubmitChanges();

                result = Json("Cliente insertado correctamente");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


                return result;
            }
            catch (Exception ex)
            {
                result = Json($"Error: {ex.Message}");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }


        }

        [HttpPost]
        public JsonResult DeleteCliente(Int64 IdCliente)
        {
            var result = Json("");

            try
            {
                var clienteSelect = from c in DataBaseHelper.Clientes where c.Id == IdCliente select c;

                if (clienteSelect.Count() == 0)
                {
                    result = Json("Error: No se ha encontrado cliente para la Id indicada");

                    result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                    return result;
                }

                var cliente = clienteSelect.First();

                DataBaseHelper.Clientes.DeleteOnSubmit(cliente);

                DataBaseHelper.SubmitChanges();

                result = Json("Cliente eliminado con exito");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }
            catch (Exception ex)
            {
                result = Json($"Error: {ex.Message}");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }
        }



        [HttpPost]
        public JsonResult UpdateCliente(Cliente newCliente)
        {

            var result = Json("");

            try
            {
                var oldClienteSelect = (from c in DataBaseHelper.Clientes where c.Id == newCliente.Id select c);

                if (oldClienteSelect.Count() == 0)
                {
                    result = Json("Error: No se ha encontrado cliente para la Id indicada");

                    result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                    return result;
                }

                var newPaisSelect = (from p in DataBaseHelper.Paises where p.Id == newCliente.IdPais select p);

                if (newPaisSelect.Count() == 0)
                {
                    result = Json("Error: No se ha encontrado pais para la Id indicada a este cliente");

                    result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                    return result;
                }

                var oldCliente = oldClienteSelect.First();

                oldCliente.Nombre = newCliente.Nombre;
                oldCliente.Direccion = newCliente.Direccion;
                oldCliente.Poblacion = newCliente.Poblacion;
                oldCliente.IdPais = newCliente.IdPais;
                oldCliente.Telefono = newCliente.Telefono;
                oldCliente.Email = newCliente.Email;


                DataBaseHelper.SubmitChanges();

                result = Json("Cliente actualizado correctamente");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;

            }
            catch (Exception ex)
            {

                result = Json($"Error: {ex.Message}");

                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return result;
            }

        }

        

        #endregion

        
    }
}