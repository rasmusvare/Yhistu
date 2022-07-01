import React from "react";
import { IMeterReading } from "../domain/IMeterReading";
import { IMeter } from "../domain/IMeter";

interface props {
  meter: IMeter;
}

const MeterReading = (data: props) => {
  const meter = data.meter;

  return (
    <div className="container mb-3">
      <h5 className="mb-2">{meter.meterType?.name}</h5>
      <div className="row d-flex justify-content-center">
        <table className="table">
          <thead>
            <tr>
              <th>Date</th>
              <th>Reading</th>
              <th>Unit</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {meter.meterReadings?.map((reading) => (
              <tr>
                <td>{reading.date.toString()}</td>
                <td>{reading.value}</td>
                <td>{meter.meterType?.measuringUnit?.symbol}</td>
                <td>
                  <a className="link">Remove</a>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};
export default MeterReading;
