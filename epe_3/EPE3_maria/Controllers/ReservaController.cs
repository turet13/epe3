
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;


[Route("Reserva")]
[ApiController]
public class ReservaController : ControllerBase
{
    // Cadena de conexión
    private readonly string _connectionString;

  
    public ReservaController(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("MySqlConnection");
    }

    // Metodo get
    [HttpGet]
    public async Task<IActionResult> ListarReserva()
    {
        try
        {
            // conexion con librerias
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // se genera copnsulta
        
                string query = "SELECT * FROM Reserva";

                // se lista los datos de Pacientes

                List<Reserva> reserva = new List<Reserva>();

              
                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (var Lector = await command.ExecuteReaderAsync())
                {
                    
                    while (await Lector.ReadAsync())
                    {
                        // se crea el metodo paciente
                        reserva.Add(new Reserva
                        {
                            idReseva = Lector.GetInt32(0),
                            Especialidad = Lector.GetString(1),
                            DiaReserva = Lector.GetString(2),
                            Paciente_idPaciente = Lector.GetInt32(3),
                        });
                    }
                }

                // respuesta 200
                return StatusCode(200, reserva);
            }
        }
        catch (Exception ex)
        {
            // error 500
            return StatusCode(500, "No fue posible listar: " + ex);
        }
    }
    

    
    // Metodo post
    [HttpPost]
    public async Task<IActionResult> Nuevomedico([FromBody] Reserva reserva)
    {
        try
        {
            
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // consulta mediante sql
                string Consulta = "INSERT INTO Paciente (Especialidad, DiaReserva, Paciente_idPaciente) VALUES (@Especialidad, @ApellidoPDiaReservaac, @Paciente_idPaciente)";

                
                using (MySqlCommand command = new MySqlCommand(Consulta, connection))
                {
                    
                    command.Parameters.AddWithValue("@Especialidad", reserva.Especialidad);
                    command.Parameters.AddWithValue("@DiaREserva", reserva.DiaReserva);
                    command.Parameters.AddWithValue("@Paciente_idPaciente", reserva.Paciente_idPaciente); 
                

                 
                    await command.ExecuteNonQueryAsync();

                    // respuesta codigo 201
                    return StatusCode(201, $"reserva creada correctamente: {reserva}");
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
    public async Task<IActionResult> ModificarMedico(int id, [FromBody] Reserva reserva)
    {
        try
        {
            
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Consulta
                string consulta = "UPDATE Reserva SET Especialidad = @Especialidad, DiaReserva= @DiaReserva, Paciente_idPaciente = @Paciente_idPaciente WHERE idPaciente = @id";

               
                using (MySqlCommand command = new MySqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@Especialidad", reserva.Especialidad);
                    command.Parameters.AddWithValue("@DiaREserva", reserva.DiaReserva);
                    command.Parameters.AddWithValue("@Paciente_idPaciente", reserva.Paciente_idPaciente);                   


                    await command.ExecuteNonQueryAsync();


                    // respuesta 200
                    return StatusCode(200, "Registro editado con exito");
                }
            }
        }

        catch (Exception ex)

        {

            // error 500
            return StatusCode(500, "No se pudo actualizar y editar la reserva : " + ex);

        }
    }

    // metodo delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarReserva(int id)

    {
        try
        {
           
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

               
                string Consulta = "DELETE FROM Reserva WHERE idReserva = @id";

              
                using (MySqlCommand command = new MySqlCommand(Consulta, connection))
                {
                    
                    command.Parameters.AddWithValue("@id", id);

                    
                    var ELim = await command.ExecuteNonQueryAsync();

                    
                    if (ELim == 0)
                    {
                        // error 404
                        return StatusCode(404, "Registro no encontrado con exito");
                    }

                    else
                    {
                        
                        return StatusCode(200, $"Reserva con el ID {id} eliminada con exito");
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