import * as React from 'react';
import { Outlet } from "react-router";
import Navbar from "./Navbar";

const HomeLayout = () => {
    return (
        <>
        <Navbar />
        <main>
            <div className="container">
                <Outlet />
            </div>
        </main>
        </>
    );
}
export default HomeLayout;