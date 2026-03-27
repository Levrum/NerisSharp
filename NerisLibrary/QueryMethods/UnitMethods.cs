using NerisLibrary.Models.ElementModels;
using NerisLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using NerisLibrary.Utils;
using System.Text.Json.Nodes;

namespace NerisLibrary
{
    public partial class NerisBase
    {
        /// <summary>
        /// Creates a new unit for the specified base entity and base station, and returns the identifier of the created unit.
        /// </summary>
        /// <param name="baseEntity">The base entity to which the unit will be added. Cannot be null, must have an Id.</param>
        /// <param name="baseStation">The base station associated with the new unit. Cannot be null, must have an Id.</param>
        /// <param name="newUnit">The unit to create. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the identifier of the newly
        /// created unit as a string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="baseEntity"/>, <paramref name="baseStation"/>, or <paramref name="newUnit"/>
        /// is null or, they do not have a valid id.</exception>
        public async Task<string> PostUnit(EntityModel baseEntity, StationModel baseStation, UnitModel newUnit)
        {
            if (baseEntity == null) throw new ArgumentNullException(nameof(baseEntity));
            if (baseStation == null) throw new ArgumentNullException(nameof(baseStation));

            return await PostUnit(baseEntity.Neris_Id, baseStation.Neris_Id, newUnit);
        }

        /// <summary>
        /// Creates a new unit for the specified base entity and base station, and returns the identifier of the created unit.
        /// </summary>
        /// <param name="baseEntityId">The unique identifier of the base entity to which the unit will be added. Cannot be null or whitespace.</param>
        /// <param name="baseStationId">The unique identifier of the base station associated with the new unit. Cannot be null or whitespace.</param>
        /// <param name="newUnit">The unit to create. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the identifier of the newly
        /// created unit as a string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="baseEntityId"/>, <paramref name="baseStationId"/>, or <paramref name="newUnit"/>
        /// is null or, for string parameters, consists only of whitespace.</exception>
        public async Task<string> PostUnit(string baseEntityId, string baseStationId, UnitModel newUnit)
        {
            if (string.IsNullOrWhiteSpace(baseEntityId)) throw new ArgumentNullException(nameof(baseEntityId));
            if (string.IsNullOrWhiteSpace(baseStationId)) throw new ArgumentNullException(nameof(baseStationId));
            if (newUnit == null) throw new ArgumentNullException(nameof(newUnit));

            string postURI = GetUnitRoute(baseEntityId, baseStationId);

            using (var message = new HttpRequestMessage(HttpMethod.Post, postURI))
            {

                string content = SerializationExtensions.SerializeLowerCase(newUnit);
                message.Content = CreateJsonContent(content);
                HttpResponseMessage response = await _call(message);
                return await ParseIdFromCreatedResult(response);
            }
        }

        /// <summary>
        /// Updates the specified unit for a given entity and station using a PATCH request, and returns the identifier
        /// of the updated unit.
        /// </summary>
        /// <remarks>Only the fields provided in unitToUpdate are updated. Fields specified in
        /// fieldsToNull are set to null in the target unit. The Neris ID of the unit cannot be changed.</remarks>
        /// <param name="baseEntity">The entity to which the unit belongs. Cannot be null, must have an id.</param>
        /// <param name="baseStation">The station associated with the unit. Cannot be null, must have an id.</param>
        /// <param name="unitToUpdate">The unit model containing the updated values. Must include a valid Neris ID.</param>
        /// <param name="fieldsToNull">A set of field names to be explicitly set to null in the update. If null, no fields are set to null.</param>
        /// <returns>A string containing the identifier of the updated unit.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="baseEntity"/>, <paramref name="baseStation"/>, or <paramref name="unitToUpdate"/> is null or their ids are null, empty, or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="unitToUpdate"/> does not contain a valid Neris ID.</exception>
        public async Task<string> PatchUnit(EntityModel baseEntity, StationModel baseStation, UnitModel unitToUpdate, HashSet<string> fieldsToNull = null)
        {
            if (baseEntity == null) throw new ArgumentNullException(nameof(baseEntity));
            if (baseStation == null) throw new ArgumentNullException(nameof(baseStation));
            return await PatchUnit(baseEntity.Neris_Id, baseStation.Neris_Id, unitToUpdate, fieldsToNull);
        }

        /// <summary>
        /// Updates the specified unit for a given entity and station using a PATCH request, and returns the identifier
        /// of the updated unit.
        /// </summary>
        /// <remarks>Only the fields provided in unitToUpdate are updated. Fields specified in
        /// fieldsToNull are set to null in the target unit. The Neris ID of the unit cannot be changed.</remarks>
        /// <param name="baseEntityId">The unique identifier of the entity to which the unit belongs. Cannot be null or whitespace.</param>
        /// <param name="baseStationId">The unique identifier of the station associated with the unit. Cannot be null or whitespace.</param>
        /// <param name="unitToUpdate">The unit model containing the updated values. Must include a valid Neris ID.</param>
        /// <param name="fieldsToNull">A set of field names to be explicitly set to null in the update. If null, no fields are set to null.</param>
        /// <returns>A string containing the identifier of the updated unit.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="baseEntityId"/>, <paramref name="baseStationId"/>, or <paramref name="unitToUpdate"/> is null or consists only of whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="unitToUpdate"/> does not contain a valid Neris ID.</exception>
        public async Task<string> PatchUnit(string baseEntityId, string baseStationId, UnitModel unitToUpdate, HashSet<string> fieldsToNull = null)
        {
            if (string.IsNullOrWhiteSpace(baseEntityId)) throw new ArgumentNullException(nameof(baseEntityId));
            if (string.IsNullOrWhiteSpace(baseStationId)) throw new ArgumentNullException(nameof(baseStationId));
            if (unitToUpdate == null) throw new ArgumentNullException(nameof(unitToUpdate));
            if (string.IsNullOrWhiteSpace(unitToUpdate.Neris_Id)) throw new ArgumentException("Unit to Update must contain Neris ID");

            string unitRoute = GetUnitRoute(baseEntityId, baseStationId, unitToUpdate.Neris_Id);

            using (var message = new HttpRequestMessage(HttpMethod.Patch, unitRoute))
            {
                JsonObject contentObj = SerializationExtensions.SerializeToJsonObjectLowerCase(unitToUpdate);
                contentObj.Remove("neris_id");
                if (fieldsToNull != null)
                {
                    foreach (string field in fieldsToNull)
                    {
                        contentObj[field.ToLower()] = null;
                    }
                }
                message.Content = CreateJsonContent(contentObj.ToJsonString());
                HttpResponseMessage response = await _call(message);

                return await ParseIdFromCreatedResult(response);
            }
            
        }

        /// <summary>
        /// Deletes the specified unit from the given base station and entity.
        /// </summary>
        /// <param name="baseEntity">The base entity containing the unit to delete. Cannot be null, must have an Id.</param>
        /// <param name="baseStation">The base station containing the unit to delete. Cannot be null, must have an Id.</param>
        /// <param name="unitToDelete">The unit to delete. Cannot be null, must have an Id.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the unit was
        /// successfully deleted; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="baseEntity"/>, <paramref name="baseStation"/>, or <paramref
        /// name="unitToDelete"/> are null or their ids are null, empty, or whitespace.</exception>
        public async Task<bool> DeleteUnit(EntityModel baseEntity, StationModel baseStation, UnitModel unitToDelete)
        {
            if (baseEntity == null) throw new ArgumentNullException(nameof(baseEntity));
            if (baseStation == null) throw new ArgumentNullException(nameof(baseStation));
            if (unitToDelete == null) throw new ArgumentNullException(nameof(unitToDelete));
            return await DeleteUnit(baseEntity.Neris_Id, baseStation.Neris_Id, unitToDelete.Neris_Id);
        }

        /// <summary>
        /// Deletes the specified unit from the given base station and entity.
        /// </summary>
        /// <param name="baseEntityId">The unique identifier of the base entity containing the unit to delete. Cannot be null or whitespace.</param>
        /// <param name="baseStationId">The unique identifier of the base station containing the unit to delete. Cannot be null or whitespace.</param>
        /// <param name="unitToDeletId">The unique identifier of the unit to delete. Cannot be null or whitespace.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the unit was
        /// successfully deleted; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="baseEntityId"/>, <paramref name="baseStationId"/>, or <paramref
        /// name="unitToDeletId"/> are null or consists only of white-space characters.</exception>
        public async Task<bool> DeleteUnit(string baseEntityId, string baseStationId, string unitToDeletId)
        {
            if (string.IsNullOrWhiteSpace(baseEntityId)) throw new ArgumentNullException(nameof(baseEntityId));
            if (string.IsNullOrWhiteSpace(baseStationId)) throw new ArgumentNullException(nameof(baseStationId));
            if (string.IsNullOrWhiteSpace(unitToDeletId)) throw new ArgumentNullException(nameof(unitToDeletId));

            string unitRoute = GetUnitRoute(baseEntityId, baseStationId, unitToDeletId);
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, unitRoute))
            {
                HttpResponseMessage response = await _call(message);
                return response.IsSuccessStatusCode;
            }
        }

        private string GetUnitRoute(string entityId, string stationId, string unitId = null)
        {
            string baseUri = GetStationRoute(entityId, stationId);
            string withUnit = UriUtils.AppendPath(baseUri, "unit");
            if (!string.IsNullOrWhiteSpace(unitId))
            {
                withUnit = UriUtils.AppendPath(withUnit, unitId);
            }
            return withUnit;
        }
    }


}
