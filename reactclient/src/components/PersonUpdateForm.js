import React, { useState } from "react";
import Constants from "../utilities/Constants";

export default function PersonUpdateForm(props) {
  const initialFormData = Object.freeze({
    id: props.person.id,
    firstName: props.person.firstName,
    lastName: props.person.lastName,
    age: props.person.age,
    address: props.person.addresses[0],
    email: props.person.emailAddresses[0]
  });

  const [formData, setFormData] = useState(initialFormData);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const personToUpdate = {
      id: props.person.id,
      firstName: formData.firstName,
      lastName: formData.lastName,
      age: formData.age,
      addresses: [formData.address],
      emailAddresses: [formData.email],
    };

    const url = `${Constants.API_URL_UPDATE_PERSON}/${props.person.id}`;

    fetch(url, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(personToUpdate),
    })
      .then((response) => response.json())
      .then((responseFromServer) => {
        console.log(responseFromServer);
      })
      .catch((error) => {
        console.log(error);
        alert(error);
      });

    props.onPersonUpdated(personToUpdate);
  };

  return (
    <form className="w-100 px-5">
      <h1 className="mt-5">Update Person</h1>

      <div className="mt-5">
        <label className="h5 form-label">FirstName</label>
        <input
          value={formData.firstName}
          name="firstName"
          type="text"
          className="form-control"
          onChange={handleChange}
        />
      </div>

      <div className="mt-5">
        <label className="h5 form-label">LastName</label>
        <input
          value={formData.lastName}
          name="lastName"
          type="text"
          className="form-control"
          onChange={handleChange}
        />
      </div>

      <div className="mt-5">
        <label className="h5 form-label">Age</label>
        <input
          value={formData.age}
          name="age"
          type="text"
          className="form-control"
          onChange={handleChange}
        />
      </div>

      <div className="mt-5">
        <label className="h5 form-label">Email</label>
        <input
          value={formData.email}
          name="email"
          type="text"
          className="form-control"
          onChange={handleChange}
        />
      </div>

      <div className="mt-5">
        <label className="h5 form-label">Address</label>
        <input
          value={formData.address}
          name="address"
          type="text"
          className="form-control"
          onChange={handleChange}
        />
      </div>

      <button onClick={handleSubmit} className="btn btn-dark btn-lg w-100 mt-5">
        Submit
      </button>
      <button
        onClick={() => props.onPersonUpdated(null)}
        className="btn btn-secondary btn-lg w-100 mt-3"
      >
        Cancel
      </button>
    </form>
  );
}
