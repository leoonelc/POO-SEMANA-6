using System;
using System.Collections.Generic;
using System.Linq;
using INVENTARIOZAP.Models;

namespace INVENTARIOZAP.Controllers
{
    public class ProductoController
    {
        private readonly DATA.MyAppDbContext _appContext;

        public ProductoController()
        {
            _appContext = new DATA.MyAppDbContext();
        }

        public string Insertar(ProductoModel productoModel)
        {
            try
            {
                productoModel.Create_At = DateTime.Now;
                productoModel.Update_Up = DateTime.Now;
                _appContext.Add(productoModel);
                _appContext.SaveChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Actualizar(ProductoModel productoModel)
        {
            var existe = _appContext.Productos.Find(productoModel.ProductoId);
            if (existe != null)
            {
                try
                {
                    existe.Nombre = productoModel.Nombre;
                    existe.Lote = productoModel.Lote;
                    existe.FechaIngreso = productoModel.FechaIngreso;
                    existe.FechaCaducidad = productoModel.FechaCaducidad;
                    existe.Proveedor = productoModel.Proveedor;
                    existe.Stock = productoModel.Stock;
                    existe.PrecioUnitario = productoModel.PrecioUnitario;
                    existe.Update_Up = DateTime.Now;
                    _appContext.SaveChanges();
                    return "ok";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                return "El producto no existe";
            }
        }

        public string Eliminar(int productoId)
        {
            try
            {
                var existe = _appContext.Productos.Find(productoId);
                if (existe != null)
                {
                    _appContext.Remove(existe);
                    _appContext.SaveChanges();
                    return "ok";
                }
                else
                {
                    return "El producto no existe";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public IEnumerable<ProductoModel> Todos() => _appContext.Productos;

        public ProductoModel Uno(int id)
        {
            var existe = _appContext.Productos.Find(id);
            if (existe != null)
            {
                return existe;
            }
            else
            {
                return new ProductoModel();
            }
        }
    }
}
