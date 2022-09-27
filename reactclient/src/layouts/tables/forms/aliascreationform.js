import React, { useState } from "react";

import { Card } from "@mui/material";
import { LoadingButton } from "@mui/lab";

import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import MDInput from "components/MDInput";

import Constants from "utilities/Constants";
import useErrorHandler from "utilities/useErrorHandler";

export default function AliasCreationForm(props) {
  /* eslint-disable react/prop-types */
    const { onPersonCreated } = props;
  const initialFormData = {};

  const [formData, setFormData] = useState(initialFormData);
  const { renderAlert, checkAndConvertResponse } = useErrorHandler();
  const [addError, setAddError] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    setLoading(true);

    const mappingToCreate = {
      name: formData.name,
      sourceAlias: formData.alias,
      targetUrl: formData.targetUrl,
      isActive: true,
      isOfficial: false,
    };

    fetch(Constants.API_URL_CREARE_MAPPING, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(mappingToCreate),
    })
      .then(checkAndConvertResponse)
      .then((responseJson) => {
        if (responseJson.isError) {
          setAddError(true);
          setLoading(false);
          return;
        }

        setFormData(responseJson);
        setAddError(false);
        setLoading(false);
      });
      /* eslint-disable react/prop-types */
    onPersonCreated(mappingToCreate);
  };

  return (
    <Card>
      <MDBox
        variant="gradient"
        bgColor="info"
        borderRadius="lg"
        coloredShadow="info"
        mx={2}
        mt={-3}
        p={2}
        mb={1}
        textAlign="center"
      >
        <MDTypography variant="h4" fontWeight="medium" color="white" mt={1}>
          Add a new alias
        </MDTypography>
      </MDBox>
      <MDBox pt={4} pb={3} px={3}>
        <MDBox component="form" role="form" onSubmit={handleSubmit}>
          <MDBox mb={2}>
            <MDInput
              type="text"
              label="Name"
              value={formData.name}
              onChange={e => handleChange(e)}
              placeholder="Friendly name..."
              fullWidth
            />
          </MDBox>
          <MDBox mb={2}>
            <MDInput
              type="text"
              label="Alias"
              value={formData.alias}
              onChange={e => handleChange(e)}
              placeholder="Custom alias name, case insensitive..."
              fullWidth
            />
          </MDBox>
          <MDBox mb={2}>
            <MDInput
              type="text"
              label="Target URL"
              value={formData.targetUrl}
              onChange={e => handleChange(e)}
              placeholder="Full url, e.g 'https://www.google.com'..."
              fullWidth
            />
          </MDBox>
          <MDBox mt={4} mb={1}>
            <LoadingButton
              color="info"
              type="submit"
              loading={loading}
              loadingPosition="start"
              variant="contained"
            >
              Add
            </LoadingButton>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <LoadingButton
              color="success"
              loading={loading}
              loadingPosition="start"
              variant="contained"
              onClick={() => onPersonCreated(null)}
            >
              Cancel
            </LoadingButton>
          </MDBox>
          {addError && renderAlert()}
        </MDBox>
      </MDBox>
    </Card>
  );
}
