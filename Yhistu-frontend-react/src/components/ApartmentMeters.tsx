import React, { useContext, useEffect, useState } from "react";
import Meter from "./Meter";
import MeterReading from "./MeterReading";
import { MeterService } from "../services/MeterService";
import { IMeter } from "../domain/IMeter";
import { ApartmentContext } from "../state/ApartmentContext";
import { useNavigate } from "react-router-dom";
import { UserContext } from "../state/UserContext";

const ApartmentMeters = () => {
  const userState = useContext(UserContext);
  const apartmentState = useContext(ApartmentContext);
  const meterService = new MeterService();
  const navigate = useNavigate();

  const [meters, setMeters] = useState([] as IMeter[]);

  const loadMeters = async () => {
    const meters = await meterService.getAll(
      apartmentState.currentApartment?.id
    );
    setMeters(meters);
  };

  useEffect(() => {
    if (userState.jwt == null) {
      navigate("/");
    }
    loadMeters();
  }, []);

  return (
    <div>
      {meters.map((each) => (
        <div key={each.id}>
          <Meter meter={each} updateReadings={loadMeters} />
          <MeterReading meter={each} />
        </div>
      ))}
    </div>
  );
};

export default ApartmentMeters;
