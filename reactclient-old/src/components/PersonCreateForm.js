import React, { useState } from "react";
import Constants from "../utilities/Constants";

export default function PersonCreateForm(props) {
    const initialFormData = {};

    const [ formData, setFormData ] = useState(initialFormData);

    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        const personToCreate = {
            firstName: formData.firstName,
            lastName: formData.lastName,
            age: formData.age,
            addresses: [ formData.address ],
            emailAddresses: [ formData.email ] 
        };

        fetch(Constants.API_URL_CREATE_PERSON, {
          method: "POST",
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(personToCreate)
        })
          .then((response) => response.json())
          .then((responseFromServer) => {
            setFormData(responseFromServer);
          })
          .catch((error) => {
            console.log(error);
          });

        props.onPersonCreated(personToCreate);
    }

    return (
        <form className="w-100 px-5">
          <h1 className="mt-5">Create Person</h1>

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

          <button
            onClick={handleSubmit}
            className="btn btn-dark btn-lg w-100 mt-5"
          >
            Submit
          </button>
          <button
            onClick={() => props.onPersonCreated(null)}
            className="btn btn-secondary btn-lg w-100 mt-3"
          >
            Cancel
          </button>
        </form>
    );
}
