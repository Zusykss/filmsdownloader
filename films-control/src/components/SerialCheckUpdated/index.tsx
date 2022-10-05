import { useEffect, useState } from "react";
import { Form } from "react-bootstrap"
import http_common from "../../http_common";
import { ISerial } from "../SerialsPage/types";

export interface ICheckedProps{
    serial : ISerial
}
const SerialCheckUpdated: React.FC<ICheckedProps> = ({serial}) => {
    const [isChecked, setIsChecked] = useState<boolean>(serial.isUpdated);
    useEffect(()=>{
        http_common.post('Serial/updateIsUpdated?id='+serial.id, isChecked);
    },[isChecked]);
    // const updateElement = (checked : boolean) => {
    //     setIsChecked(serial.isUpdated);
    //     console.log("upd");
    //     setIsChecked(checked);
    // }
    //useEffect()
    return (
        <Form.Check
            inline
            name="group1"
            checked={isChecked}
            onChange={(ev) => { setIsChecked(ev.target.checked);}}
            type='checkbox'
          />
    )
}
export default SerialCheckUpdated;