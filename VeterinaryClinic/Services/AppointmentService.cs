using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Services
{
    internal class AppointmentService
    {
        private readonly AppointmentRepository appointmentRepository = new AppointmentRepository();

        public void CreateAppointment(string animalName, string veterinarianName, DateTime dateTime,string purpose)
        {
            if (dateTime < DateTime.Now)
            {
                throw new ArgumentException("An appointment cannot be scheduled in the past.");
            }

            int journalId = appointmentRepository.GetJournalIdByAnimalName(animalName);

            int veterinarianId = appointmentRepository.GetVeterinarianIdByName(veterinarianName);

            int appointmentId = appointmentRepository.CreateAppointment(journalId, veterinarianId, dateTime, purpose);

            FileLogger.Log("INSERT", "Appointment", appointmentId, "-", $"JournalId: {journalId}, VeterinarianId: {veterinarianId}, DateTime: {dateTime}, Purpose: {purpose}");
        }

        public void GetMedicalHistory(string animalName)
        {
            if (string.IsNullOrWhiteSpace(animalName))
            {
                throw new ArgumentException("Animal name cannot be empty.");
            }

            appointmentRepository.GetAnimalMedicalHistory(animalName);

            FileLogger.Log("READ", "Appointment", 0, "-", $"Read medical history for animal: {animalName}");
        }

        public void UpdateAppointment(int appointmentId, DateTime newDateTime)
        {
            if (newDateTime < DateTime.Now)
            {
                throw new ArgumentException("An appointment cannot be rescheduled to the past.");
            }

            DateTime oldDateTime = appointmentRepository.GetAppointmentDateTime(appointmentId);

            appointmentRepository.UpdateAppointment(appointmentId, newDateTime);

            FileLogger.Log("UPDATE", "Appointment", appointmentId, $"DateTime: {oldDateTime}", $"DateTime: {newDateTime}");
        }

        public void DeleteAnimal(int animalId)
        {
            bool hasMedicalHistory = appointmentRepository.AnimalHasMedicalHistory(animalId);

            if (hasMedicalHistory)
            {
                throw new InvalidOperationException("An animal with existing medical history cannot be deleted.");
            }

            string oldData = appointmentRepository.GetAnimalData(animalId);

            appointmentRepository.DeleteAnimal(animalId);

            FileLogger.Log("DELETE", "Animal", animalId, oldData, "-");
        }

        public void ShowAnimal(string animalName)
        {
            appointmentRepository.ShowAnimal(animalName);
        }
    }
}
