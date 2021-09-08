using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.DTOs;
using WebApi_ComprasStock.DTOs.Diccionarios;
using WebApi_ComprasStock.DTOs.Productos;
using WebApi_ComprasStock.DTOs.ProveedoresDTOs;
using WebApi_ComprasStock.Entidades;

namespace WebApi_ComprasStock.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ApplicationUser, UsuarioDTO>();
            CreateMap<IdentityRole, RolesDTO>();

            //--------------------------------------------------------------------------------------------
            CreateMap<ProveedorDTO, DatosProveedores>();

            CreateMap<DatosProveedores, ProveedorDTO>()
                .ForMember(proveedor=> proveedor.diccionario_CIVA, options => options.MapFrom(MapCIVAEnProveedor));

            CreateMap<DatosProveedores, ProveedorDTOConProductos>()
                .ForMember(proveedorDTO => proveedorDTO.Productos, options => 
                options.MapFrom(MapearProveedoresProductos));

            CreateMap<ProveedorCreacionDTO, DatosProveedores>()
                .ForMember(x => x.ProveedorProducto, options => options.MapFrom(MapearProductosIds));
            //--------------------------------------------------------------------------------------------
            CreateMap<ProductoDTO, Producto>();
            CreateMap<ProductoDTOCompleto, Producto>();

            CreateMap<Producto, ProductoDTO>();

            CreateMap<Producto, ProductoDTOCompleto>()
                .ForMember(x => x.Proveedores, options => options.MapFrom(MapearProductosProveedores))
                .ForMember(tipo => tipo.TipoProducto , options => options.MapFrom(MapearProductosTipo));

            CreateMap<ProductoCreacionDTO, Producto>()
                .ForMember(x => x.ProveedorProducto, options => options.MapFrom(MapearProveedoresIds));

            CreateMap<ProductoPatchDTO, Producto>().ReverseMap();
            //--------------------------------------------------------------------------------------------
            CreateMap<TipoProducto, TipoProductoDTO>().ReverseMap();

            CreateMap<TipoProducto, TipoProductoDTOCompleta>()
                .ForMember(x => x.Categoria, options => options.MapFrom(MapCategoriaEnTipoProd))
                .ForMember(x => x.Productos, options => options.MapFrom(MapProductosEnTipoProd));

            CreateMap<TipoProductoCreacionDTO, TipoProducto>();
            //--------------------------------------------------------------------------------------------
            CreateMap<Categorias, CategoriaDTO>().ReverseMap();

            CreateMap<Categorias, CategoriaDTOCompleta>()
                .ForMember(x => x.Rubro, options => options.MapFrom(MapearRubroCategoria))
                .ForMember(x => x.TipoProductos, options => options.MapFrom(MapearTiposCategoria));

            CreateMap<CategoriaCreacionDTO, Categorias>();
            //--------------------------------------------------------------------------------------------
            CreateMap<Rubros, RubroDTO>().ReverseMap();

            CreateMap<Rubros, RubroDTOCompleta>()
                .ForMember(x => x.Categorias, options => options.MapFrom(MapCategoriaRubroCompleto));

            CreateMap<RubroCreacionDTO, Rubros>();
            //--------------------------------------------------------------------------------------------
            CreateMap<Diccionario_CIVA, Diccionario_CIVA_DTO>().ReverseMap();

            CreateMap<Diccionario_CIVA_CreacionDTO, Diccionario_CIVA>();
            //--------------------------------------------------------------------------------------------
            CreateMap<UnidadDeMedida, UnidadDeMedidaDTO>().ReverseMap();

            CreateMap<UnidadDeMedidaCreacionDTO, UnidadDeMedida>();
            //--------------------------------------------------------------------------------------------
        }
        //_________________________________________________________________________________________________________________________________
        private Diccionario_CIVA_DTO MapCIVAEnProveedor (DatosProveedores proveedor, ProveedorDTO proveedorDTO)
        {
            var resultado = new Diccionario_CIVA_DTO();
            if(proveedor.Diccionario_CIVA != null)
            {
                resultado.Activado = proveedor.Diccionario_CIVA.Activo;
                resultado.Descripcion = proveedor.Diccionario_CIVA.Descripcion;
                resultado.Id = proveedor.Diccionario_CIVA.Id;
            }

            return resultado;
        }
        //_________________________________________________________________________________________________________________________________
        private List<ProductoDTO> MapProductosEnTipoProd(TipoProducto tipoProd, TipoProductoDTOCompleta tipoDTOCompleta)
        {
            var resultado = new List<ProductoDTO>();
            if(tipoProd.Productos != null)
            {
                foreach(var productosEnTipo in tipoProd.Productos)
                {
                    resultado.Add(new ProductoDTO()
                    {
                        Borrado = productosEnTipo.Borrado,
                        CodigoBarra = productosEnTipo.CodigoBarra,
                        Descripcion = productosEnTipo.Descripcion,
                        Id = productosEnTipo.Id,
                        Imagen = productosEnTipo.Imagen,
                        TipoProductoId = tipoProd.Id,
                        UnidadMedida = productosEnTipo.UnidadMedida
                    });
                }
            }

            return resultado;
        }
        //_________________________________________________________________________________________________________________________________
        private CategoriaDTO MapCategoriaEnTipoProd(TipoProducto tipoProd, TipoProductoDTOCompleta tipoDTOCompleta)
        {
            var resultado = new CategoriaDTO();
            if(tipoProd.Categorias != null)
            {
                resultado.CodigoCategoria = tipoProd.Categorias.CodigoCategoria;
                resultado.Descripcion = tipoProd.Categorias.Descripcion;
                resultado.Id = tipoProd.Categorias.Id;
            }
            return resultado;
        }
        //_________________________________________________________________________________________________________________________________
        private List<CategoriaDTO> MapCategoriaRubroCompleto(Rubros rubro,RubroDTOCompleta rubroDTOCompleta)
        {
            var resultado = new List<CategoriaDTO>();
            if(rubro.Categorias != null)
            {
                foreach(var categoriasenrubros in rubro.Categorias)
                {
                    resultado.Add(new CategoriaDTO() { 
                        CodigoCategoria = categoriasenrubros.CodigoCategoria,
                        Descripcion= categoriasenrubros.Descripcion,
                        Id=categoriasenrubros.Id
                    });
                }
            }
            return resultado;
        }
        //_________________________________________________________________________________________________________________________________
        private List<TipoProductoDTO> MapearTiposCategoria(Categorias categoria, CategoriaDTOCompleta categoriaDTOCompleta)
        {
            var resultado = new List<TipoProductoDTO>();

            if (categoria.TipoProductos != null)
            {
                foreach (var categoriasTipos in categoria.TipoProductos)
                {
                    resultado.Add(new TipoProductoDTO()
                    {
                        Id = categoriasTipos.Id,
                        CodigoTipo= categoriasTipos.CodigoTipo,
                        Descripcion= categoriasTipos.Descripcion
                    });
                }
            }

            return resultado;
        }
        //_________________________________________________________________________________________________________________________________
        private RubroDTO MapearRubroCategoria(Categorias categoria, CategoriaDTOCompleta categoriaDTOCompleta)
        {
            var resultado = new RubroDTO();

            if (categoria.Rubro != null)
            {
                resultado = new RubroDTO()
                {
                    Id = categoria.Rubro.Id,
                    Descripcion = categoria.Rubro.Descripcion,
                    CodigoRubro= categoria.Rubro.CodigoRubro
                };
            }

            return resultado;
        }
        //_________________________________________________________________________________________________________________________________
        private TipoProductoDTO MapearProductosTipo(Producto producto, ProductoDTO productoDTO)
        {
            var resultado = new TipoProductoDTO();

            if (producto.TipoProducto != null)
            {
                resultado = new TipoProductoDTO()
                {
                    Id = producto.TipoProductoId,
                    CodigoTipo= producto.TipoProducto.CodigoTipo,
                    Descripcion=producto.TipoProducto.Descripcion
                };
            }

            return resultado;
        }
        //_________________________________________________________________________________________________________________________________
        private List<ProductoDTO> MapearProveedoresProductos(DatosProveedores proveedor, ProveedorDTO proveedorDTO)
        {
            var resultado = new List<ProductoDTO>();

            if (proveedor.ProveedorProducto != null)
            {
                foreach (var proveedoresProducto in proveedor.ProveedorProducto)
                {
                    resultado.Add(new ProductoDTO()
                    {
                        Id = proveedoresProducto.ProductoId,
                        Borrado= proveedoresProducto.Producto.Borrado,
                        CodigoBarra=proveedoresProducto.Producto.CodigoBarra,
                        Descripcion = proveedoresProducto.Producto.Descripcion,
                        Imagen = proveedoresProducto.Producto.Imagen,
                        TipoProductoId = proveedoresProducto.Producto.TipoProductoId,
                        UnidadMedida = proveedoresProducto.Producto.UnidadMedida
                    });
                }
            }

            return resultado;
        }
        //_________________________________________________________________________________________________________________________________
        private List<ProveedorDTO> MapearProductosProveedores(Producto producto, ProductoDTO productoDTO)
        {
            var resultado = new List<ProveedorDTO>();

            if (producto.ProveedorProducto != null)
            {
                foreach (var productosProveedores in producto.ProveedorProducto)
                {
                    resultado.Add(new ProveedorDTO()
                    {
                        Id = productosProveedores.ProveedorId,
                        Activo= productosProveedores.DatosProveedores.Activo,
                        CIVA=productosProveedores.DatosProveedores.CIVAId,
                        Cuit=productosProveedores.DatosProveedores.Cuit,
                        Direccion=productosProveedores.DatosProveedores.Direccion,
                        Email=productosProveedores.DatosProveedores.Email,
                        RSocial=productosProveedores.DatosProveedores.RSocial,
                        Telefonos=productosProveedores.DatosProveedores.Telefonos,
                        Web=productosProveedores.DatosProveedores.Web
                    });
                }
            }

            return resultado;
        }
        //_________________________________________________________________________________________________________________________________
        private List<ProveedorProducto> MapearProveedoresIds(ProductoCreacionDTO productoCreacionDTO,Producto producto )
        {
            var resultado = new List<ProveedorProducto>();
            if(productoCreacionDTO.ProveedoresIds?.Count > 0)
            {
                foreach(var proveedorId in productoCreacionDTO.ProveedoresIds)
                {
                    resultado.Add(new ProveedorProducto()
                    {
                        ProveedorId = proveedorId
                    });
                }
            }
            return resultado;
        }
        //_________________________________________________________________________________________________________________________________
        // 
        private List<ProveedorProducto> MapearProductosIds(ProveedorCreacionDTO proveedorCreacionDTO, DatosProveedores datosProveedores)
        {
            var resultado = new List<ProveedorProducto>();
            if (proveedorCreacionDTO.ProductosIds?.Count > 0)
            {
                foreach (var productoId in proveedorCreacionDTO.ProductosIds)
                {
                    resultado.Add(new ProveedorProducto()
                    {
                        ProveedorId = productoId
                    });
                }
            }
            return resultado;
        }
        //_________________________________________________________________________________________________________________________________
    }
}