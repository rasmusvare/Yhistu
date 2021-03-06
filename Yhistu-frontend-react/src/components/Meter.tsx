import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { IMeter } from "../domain/IMeter";
import { AssociationService } from "../services/AssociationService";
import { MeterService } from "../services/MeterService";
import { MeterReadingService } from "../services/MeterReadingService";
import { IMeterReading } from "../domain/IMeterReading";
import meterReading from "./MeterReading";

interface props {
  meter: IMeter;
  updateReadings: any;
}

const Meter = (data: props) => {
  const meterService = new MeterService();
  const meterReadingService = new MeterReadingService();

  const meter = data.meter;
  const [meterReadings, setMeterReadings] = useState([] as IMeterReading[]);

  const [readingData, setReadingData] = useState({
    value: "",
  });

  const [errorMessages, setErrorMessages] = useState(
    [] as string[] | undefined
  );

  const addReading = async (e: Event) => {
    const res = await meterReadingService.add({
      meterId: meter.id!,
      value: parseInt(readingData.value),
      date: new Date().toDateString(),
      autoGenerated: false,
    });
    if (res.status >= 300) {
      setErrorMessages(res.errorMessage);
      console.log(res);
    } else {
      setErrorMessages(undefined);
      data.updateReadings();
    }
  };

  const loadMeterReadings = async () => {
    const meterReadings = await meterReadingService.getAll(meter.id);
    setMeterReadings(meterReadings);
  };

  useEffect(() => {
    loadMeterReadings();
  }, []);

  return (
    <>
      {errorMessages?.map((each) => (
        <div
          className="text-danger validation-summary-errors mt-3 text-center"
          data-valmsg-summary="true"
          data-valmsg-replace="true"
        >
          <ul key={each}>
            <li>{each}</li>
          </ul>
        </div>
      ))}
      <div className="container d-flex justify-content-center mb-3 h-100">
        <div className="col-8 h-100">
          <div className="card h-100 border-secondary justify-content-center">
            <div className="card-body">
              <h3 className="card-title text-center mb-1">
                {meter.meterType?.name}
              </h3>
              <div className="text-center mb-1">
                <small>({meter.meterNumber})</small>
              </div>
              <div className="row align-items-center justify-content-center">
                <div className="form-floating col-auto">
                  <input
                    value={readingData.value}
                    onChange={(e) =>
                      setReadingData({
                        ...readingData,
                        value: e.target.value,
                      })
                    }
                    className="form-control"
                    type="number"
                  />
                  <label>Reading</label>
                </div>
                <div className="col-auto">
                  <h5>{meter.meterType?.measuringUnit?.symbol}</h5>
                </div>
                <div className="col-auto">
                  <input
                    onClick={(e) => addReading(e.nativeEvent)}
                    type="submit"
                    value="Add reading"
                    className="btn btn-outline-secondary btn-lg"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Meter;
