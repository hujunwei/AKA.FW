/**
=========================================================
* Material Dashboard 2 React - v2.1.0
=========================================================

* Product Page: https://www.creative-tim.com/product/material-dashboard-react
* Copyright 2022 Creative Tim (https://www.creative-tim.com)

Coded by www.creative-tim.com

 =========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
*/

import { useEffect, useState } from "react";

// @mui material components
import Card from "@mui/material/Card";
import Icon from "@mui/material/Icon";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";

// Material Dashboard 2 React components
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";

// Material Dashboard 2 React examples
import DataTable from "examples/Tables/DataTable";

// Data
import Constants from "utilities/Constants";
import useToken from "utilities/UseToken";
import renderProgress from "utilities/renderProgress";
import useErrorHandler from "utilities/useErrorHandler";

function Projects() {
  const [menu, setMenu] = useState(null);

  const openMenu = ({ currentTarget }) => setMenu(currentTarget);
  const closeMenu = () => setMenu(null);

  const renderMenu = (
    <Menu
      id="simple-menu"
      anchorEl={menu}
      anchorOrigin={{
        vertical: "top",
        horizontal: "left",
      }}
      transformOrigin={{
        vertical: "top",
        horizontal: "right",
      }}
      open={Boolean(menu)}
      onClose={closeMenu}
    >
      <MenuItem onClick={closeMenu}>Action</MenuItem>
      <MenuItem onClick={closeMenu}>Another action</MenuItem>
      <MenuItem onClick={closeMenu}>Something else</MenuItem>
    </Menu>
  );

  const columns = [
    { Header: "name", accessor: "name", width: "40%", align: "left" },
    { Header: "alias", accessor: "alias", width: "20%", align: "left" },
    { Header: "targeturl", accessor: "targeturl", width: "20%", align: "left" },
    { Header: "isactive", accessor: "isactive", width: "20%", align: "center" },
  ];
  const { token } = useToken();
  const [ loading, setLoading ] = useState(false);
  const [ loadOfficialUrlsError, setLoadOfficialUrlsError ] = useState(false);
  const { renderAlert, checkAndConvertResponse } = useErrorHandler();
  const [ tableData, setTableData ] = useState([]);
  
  async function loadOfficialUrls() {
    return fetch(Constants.API_URL_LIST_OFFCIAL_MAPPINGS, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    })
      .then(checkAndConvertResponse)
      .then((responseJson) => {
        setLoading(false);

        if (responseJson.isError) {
          setLoadOfficialUrlsError(true);  
          return;
        }

        setLoadOfficialUrlsError(false);      
        setTableData(responseJson);   
      });
  }

  useEffect(async () => {
    setLoading(true);
    await loadOfficialUrls();
  }, []);

  const rows = tableData.map(url => ({
      name: (
        <MDTypography variant="caption" color="text" fontWeight="medium">
          {url.name}
        </MDTypography>
      ),
      alias: (
        <MDTypography variant="caption" color="text" fontWeight="medium">
          {url.sourceAlias}
        </MDTypography>
      ),
      targeturl: (
        <MDTypography variant="caption" color="text" fontWeight="medium">
          {url.targetUrl}
        </MDTypography>
      ),
      isactive: (
        <MDTypography variant="caption" color="text" fontWeight="medium">
          {url.isActive ? "true" : "false"}
        </MDTypography>
      )
  }));

  return (
    <Card>
      <MDBox display="flex" justifyContent="space-between" alignItems="center" p={3}>
        <MDBox>
          <MDTypography variant="h6" gutterBottom>
            Official HOT alias links @ FREEWHEEL
          </MDTypography>
        </MDBox>
        <MDBox color="text" px={2}>
          <Icon sx={{ cursor: "pointer", fontWeight: "bold" }} fontSize="small" onClick={openMenu}>
            more_vert
          </Icon>
        </MDBox>
        {renderMenu}
      </MDBox>
      {loading && renderProgress()}
      {loadOfficialUrlsError && renderAlert()}
      <MDBox>
        <DataTable
          table={{ columns, rows }}
          showTotalEntries={false}
          isSorted={false}
          noEndBorder
          entriesPerPage={false}
        />
      </MDBox>
    </Card>
  );
}

export default Projects;
