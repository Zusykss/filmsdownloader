import axios from 'axios';
import { Form, FormikProvider, useFormik } from 'formik';
import React, { useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Swal from 'sweetalert2';
import IParserStartParams, { IPlatform } from './types';
import './index.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowRotateForward } from '@fortawesome/free-solid-svg-icons'; 
import PlatformItem from '../platformItem';
import { Spinner } from 'react-bootstrap';
import http_common from '../../http_common';
export default function HomePage(props: any) {
    const initialValues: IParserStartParams = {
        //count: 100,
        category: 1,
        platforms: []
    };
    const navigate = useNavigate();
    const [parserState, setParserState]= useState<string>("Unkown");

    const [platforms, setPlatforms] = useState<IPlatform[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    //const infinityRef = useRef<any>(null);
    const [isInfinity, setIsInfinity] = useState(true);

    const onHandleSubmit = async (values: IParserStartParams) => {
      try{
        values.platforms = platforms.filter(pl => pl.isSelected).map(pl => pl.id);
        const model = {...values};
        //console.log("");
        //console.log(values);
        if(parserState === 'Enabled'){
          http_common
          .get("Parser/stopParser")
          .then((data) => {
            console.log("data", data);
            Swal.fire({
              icon: "success",
              title: "Nice!",
              text: "Parser stoped!",
            });
            updateParserState();
            navigate('/');
          })
          .catch((err) => {
            Swal.fire({
              icon: "error",
              title: "Oops...",
              text: "Something went wrong!",
            });
            console.log(err);
          });
          
        }
        else if(parserState === 'Disabled'){
          http_common
          .post("Parser/startParser", model)
          .then((data) => {
            console.log("data", data);
            Swal.fire({
              icon: "success",
              title: "Nice!",
              text: "Parser started!",
            });
            updateParserState();
            navigate('/');
          })
          .catch((err) => {
            Swal.fire({
              icon: "error",
              title: "Oops...",
              text: "Something went wrong!",
            });
            console.error(err);
          });
        }
        
      }
      catch(error){
        console.error("problem submit", error);
      }
      //console.log(values);
  };

    const formik = useFormik({
      initialValues: initialValues,
      // validationSchema: RegisterSchema,
      onSubmit: onHandleSubmit,
    });
    const { errors, touched, handleSubmit, handleChange, setFieldValue } = formik;
    useEffect(() => {
        setLoading(true);
        //console.log(process.env.REACT_APP_SERVER_URL+'1');
        // if(infinityRef.current != null){
        //   infinityRef.current.focus();
        // }
        //inout
        //infinityRef.current!.focus();
        //console.log('updated!');
        updateParserState();
        http_common.get<IPlatform[]>("Platform/getAllPlatforms").then((data)=>{
            //console.log(data.data);
            //data.data = 
            //data.data.shift();
            data.data.splice(data.data.findIndex(p => p.id === 1), 1);
            setPlatforms(data.data);
        }).catch(ex => {
          Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Something went wrong!",
          });
        }).finally(() => setLoading(false));
    },[]);
    //if (loading) return <Spinner animation="border"/>;
    
    const updateParserState = () => {
      http_common
        .get("Parser/getParserState")
        .then((data) => {
          //console.log("");
          setParserState(data.data === 1 ? "Enabled" : "Disabled");
        })
        .catch((err) => {
          setParserState("Unkown");
          Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Something went wrong!",
          });
        });
    };
    // useEffect(() => {
    //   console.log("changes " + platforms[1]?.isSelected);
    // }, [platforms])
    
    
      // useEffect(()=> {
      //   infinityRef.current.checked = formik.values.count == null;
      // },[formik.values.count]);
      const updatePlatforms = (el : IPlatform) =>{
        //, platforms[key] = platforms[key]
        //el.isSelected = !el.isSelected;
        const newPlatforms : IPlatform[] = platforms.slice();
        const elementIndex : number | undefined = newPlatforms.findIndex(el2 => el2.id === el.id);
        if(elementIndex !== undefined){
          //console.log("must be "+!newPlatforms[elementIndex].isSelected+ " have "+newPlatforms[elementIndex].isSelected);
          //console.log("was : " + newPlatforms[elementIndex].isSelected + " at index : " + elementIndex);
          newPlatforms[elementIndex].isSelected = !newPlatforms[elementIndex].isSelected;
          //console.log("became : " + newPlatforms[elementIndex].isSelected );
          //console.log("NEW", newPlatforms);
          setPlatforms(newPlatforms);
          //console.log("became2 : " + platforms[elementIndex].isSelected );
        }
      }
     
  return (

    <div className="row p-5">
      { loading ? <Spinner animation="border" className="p-5 text-center"></Spinner> : (
        <div className="offset-md-3 col-md-6">
        <button className='btn btn-primary mb-2' onClick={() => updateParserState()}> Refresh   <FontAwesomeIcon icon={faArrowRotateForward} /> </button>
        <h3> State: {parserState} </h3>
        {
          parserState === "Started" ? (
            <button className='btn btn-primary'></button>
          ) : null
        }
        <FormikProvider value={formik}>
          <Form onSubmit={handleSubmit}>
            <div className="mb-3">
              <label htmlFor="categoryBtn" className="form-label">
                Parse type: 
              </label>
              <button
                type="button"
                name="categoryBtn"
                id="categoryBtn"
                className="btn btn-primary mx-3"
                onClick={() => {
                  setFieldValue(
                    "category",
                    formik.values.category === 1 ? 0 : 1
                  );
                }}
              >
                {formik.values.category === 1 ? "Movie" : "Serials"}
              </button>
              {/* {touched.email && errors.email && (
                <div className="invalid-feedback">{errors.email}</div>
              )} */}
            </div>
            <div className="mb-3">
              <label className="form-check-label" htmlFor="flexCheckDefault">
                Infinity
              </label>
              <input
                className="form-check-input mx-2 p-2"
                type="checkbox"
                id="flexCheckDefault"
                // onChange={}
                // value=
                //defaultChecked={formik.values.count == null}
                checked={isInfinity}
                //ref={infinityRef}
                onClick={() => {
                  setIsInfinity(!isInfinity);
                  setFieldValue('count', isInfinity ? 100 : null) ;
                  //console.log(formik.values.count);
              }
              }
              >
                </input>
            </div>
            {
              !isInfinity ? (
              <div className="mb-3 d-flex flex-column">
              <label htmlFor="count" className="form-label">
                Count of tv/movies:
              </label>
              <input
                type="number"
                value={formik.values.count}
                min='1'
                onChange={(ev) => {if(!isInfinity) {setFieldValue('count', ev.target.value.replace(/[^0-9]/g, ""))}}}
                name="count"
                id="count"
                pattern="[1-9][0-9]*"
              >
              </input>
            </div>
              ):
              (
                <h3>Infinity count of parsing</h3>
              )
            }
            <div className="platforms">
              {platforms.map((el: IPlatform) => (
                <img 
                key = {el.id}
                src={el?.imageUrl} 
                alt={el?.name} 
                className={el.isSelected ? "selectedImage" : "nonSelected"}
                onClick={() => {updatePlatforms(el); }} //console.log(el.isSelected)
                ></img>
              ))}
            </div>

            <button type="submit" className="btn btn-primary" disabled={ parserState === "Unkown"}>
              { parserState === 'Enabled' ? 'Stop' : 'Start'}
            </button>
          </Form>
        </FormikProvider>
      </div>
      )}
    </div>
  );
}