// Importación de namespaces necesarios
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;


[Route("Paciente")]
[ApiController]
public class PacienteController : ControllerBase
{
    // Cadena de conexión a la base de datos
    private readonly string _connectionString;

    // Constructor del controlador que recibe la configuración (por inyección de dependencias)
    public PacienteController(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("MySqlConnection");
    }

    // Metodo get
    [HttpGet]
    public async Task<IActionResult> ListarPacientes()
    {
        try
        {
            // conexion con librerias
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // se genera copnsulta
        
                string query = "SELECT * FROM Paciente";

                // se lista los datos de Pacientes

                List<Paciente> paciente = new List<Paciente>();

              
                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (var Lector = await command.ExecuteReaderAsync())
                {
                    
                    while (await Lector.ReadAsync())
                    {
                        // se crea el metodo paciente
                        paciente.Add(new Paciente
                        {
                            id_Paciente = Lector.GetInt32(0),
                            NombrePac = Lector.GetString(1),
                            ApellidoPac = Lector.GetString(2),
                            RunPac = Lector.GetString(3),
                            Nacionalidad = Lector.GetString(4),
                            Visa = Lector.GetString(5),
                            genero = Lector.GetString(6),
                            Sintomas = Lector.GetString(7),
                            Medico_idMedico = Lector.GetInt32(8)
                        });
                    }
                }

                // respuesta 200
                return StatusCode(200, paciente);
            }
        }
        catch (Exception ex)
        {
            // error 500
            return StatusCode(500, "No fue posible listar: " + ex);
        }
    }
    

    // consulta por id
    [HttpGet("{idPaciente}")]
    public async Task<IActionResult> ObtenerPaciente(int id)
    {
        try
        {
            
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Consulta SQL para obtener un médico por su ID
                string Consulta = "SELECT * FROM Paciente WHERE id_Paciente = @id";

                
                Paciente paciente = new Paciente();

                
                using (MySqlCommand command = new MySqlCommand(Consulta, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                   
                    using (var Lector = await command.ExecuteReaderAsync())
                    {
                       // validador de dato
                        if (await Lector.ReadAsync())
                        {
                           
                            paciente.id_Paciente = Lector.GetInt32(0);
                            paciente.NombrePac = Lector.GetString(1);
                            paciente.ApellidoPac = Lector.GetString(2);
                            paciente.RunPac = Lector.GetString(3);
                            paciente.Nacionalidad = Lector.GetString(4);
                            paciente.Visa = Lector.GetString(5);
                            paciente.genero = Lector.GetString(6);
                            paciente.Sintomas = Lector.GetString(7);
                            paciente.Medico_idMedico = Lector.GetInt32(8);

                            //respuesta 200
                            return StatusCode(200, paciente);
                        }
                        else
                        {
                            //error 400

                            return StatusCode(404, "No se encontro");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // erro 500
            return StatusCode(500, "No se puede realizar: " + ex);
        }
    }

    // Metodo post
    [HttpPost]
    public async Task<IActionResult> Nuevomedico([FromBody] Paciente paciente)
    {
        try
        {
            // Uso de "using" para garantizar la liberación de recursos
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // consulta mediante sql
                string Consulta = "INSERT INTO Paciente (NombrePac, ApellidoPac, RunPac, Nacionalidad, Visa, genero, SintomasPac, Medico_idMedico) VALUES (@NombrePac, @ApellidoPac, @RunPac, @Nacionalidad, @visa, @genero, @SintomasPac, @Medico_idMedico)";

                
                using (MySqlCommand command = new MySqlCommand(Consulta, connection))
                {
                    
                    command.Parameters.AddWithValue("@NombrePac", paciente.NombrePac);
                    command.Parameters.AddWithValue("@ApellidoPac", paciente.ApellidoPac);
                    command.Parameters.AddWithValue("@RunPac", paciente.RunPac);
                    command.Parameters.AddWithValue("@Nacionalidad", paciente.Nacionalidad);
                    command.Parameters.AddWithValue("@Visa", paciente.Visa);
                    command.Parameters.AddWithValue("@Genero", paciente.genero);
                    command.Parameters.AddWithValue("@SintomasPac", paciente.Sintomas);
                    command.Parameters.AddWithValue("@Medico_idMedico", paciente.Medico_idMedico);

                 
                    await command.ExecuteNonQueryAsync();

                    // respuesta codigo 201
                    return StatusCode(201, $"Paciente creado correctamente: {paciente}");
                }
            }
        }
        catch (Exception ex)
        {
            // error 500
            return StatusCode(500, "No se pudo guardar  " + ex);
        }
    }

    // metodo put
    [HttpPut("{id}")]
    public async Task<IActionResult> ModificarMedico(int id, [FromBody] Paciente paciente)
    {
        try
        {
            
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Consulta
                string consulta = "UPDATE Paciente SET NombrePac = @NombrePac, ApellidoPac= @ApellidoPac, RunPac = @RunPac, Nacionalidad = @Nacionalidad, Visa = @Visa, genero = @genero, SintomasPac = @SintomasPac, Medico_idMedico = @Medico_idMedico WHERE idPaciente = @id";

               
                using (MySqlCommand command = new MySqlCommand(consulta, connection))
                {
                 
                    command.Parameters.AddWithValue("@NombrePac", paciente.NombrePac);
                    command.Parameters.AddWithValue("@ApellidoPac", paciente.ApellidoPac);
                    command.Parameters.AddWithValue("@RunPac", paciente.RunPac);
                    command.Parameters.AddWithValue("@Nacionalidad", paciente.Nacionalidad);
                    command.Parameters.AddWithValue("@visa", paciente.Visa);
                    command.Parameters.AddWithValue("@Genero", paciente.genero);
                    command.Parameters.AddWithValue("@SintomasPac", paciente.Sintomas);
                    command.Parameters.AddWithValue("@Medico_idMedico", paciente.Medico_idMedico);
                    command.Parameters.AddWithValue("@id", id);

                   


                    await command.ExecuteNonQueryAsync();


                    // respuesta 200
                    return StatusCode(200, "Registro editado con exito");
                }
            }
        }

        catch (Exception ex)

        {

            // error 500
            return StatusCode(500, "No se pudo actualizar y editar al medico : " + ex);

        }
    }

    // metodo delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarMedico(int id)

    {
        try
        {
           
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Consulta SQL para eliminar un médico por su ID
                string Consulta = "DELETE FROM Medico WHERE idMedico = @id";

                // Uso de "using" para garantizar la liberación de recursos
                using (MySqlCommand command = new MySqlCommand(Consulta, connection))
                {
                    // Asignación de parámetros
                    command.Parameters.AddWithValue("@id", id);

                    // Ejecución de la consulta de eliminación
                    var ELim = await command.ExecuteNonQueryAsync();

                    // Verificación de si se eliminó algún registro
                    if (ELim == 0)
                    {
                        // error 404
                        return StatusCode(404, "Registro no encontrado con exito");
                    }

                    else
                    {
                        
                        return StatusCode(200, $"Medico con el ID {id} eliminado correctamente");
                    }
                }
            }
        }

        catch (Exception ex)
        {
            // error 500
            return StatusCode(500, "No se pudo eliminar : " + ex);

        }
    }
}