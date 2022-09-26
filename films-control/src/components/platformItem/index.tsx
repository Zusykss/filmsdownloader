 const PlatformItem = (props : any) => {
                return(
                    <img 
                src={props.platform?.imageUrl} 
                alt={props.platform?.name} 
                className={props.platform.isSelected ? "selectedImage" : "nonSelected"}
                onClick={() => {props.platform.isSelected = !props.platform.isSelected; console.log(props.platform.isSelected)}}
                // className={classNames(
                //   "selectedImage"
                //   {}
                // )}
                ></img>
                )
}
export default PlatformItem;