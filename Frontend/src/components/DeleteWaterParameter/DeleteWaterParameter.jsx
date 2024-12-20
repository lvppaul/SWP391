import { BiTrash } from 'react-icons/bi'
import { deleteWaterData } from '../../Config/WaterParameterApi'
import { useState } from 'react'
import { showConfirmAlert } from '../ConfirmAlert/ConfirmAlert'

const DeleteWaterParameter = ({waterData, updateDeleteWaterData}) => {
    const [measureId, setMeasureId] = useState(waterData);

    const handleDeleteWaterData = async () => {
        if(measureId){
            try{
                const result = await showConfirmAlert('Delete WaterData', 'Are you sure you want to delete this water data?');
                if(result){
                    const response = await deleteWaterData(measureId)
                    if (response) {
                        console.log('WaterData deleted successfully', response);
                        updateDeleteWaterData(measureId);
                    }
                }
            } catch (error) {
                console.error('Error deleting WaterData:', error);
            }
        }
    }

  return (
    <button onClick={handleDeleteWaterData}
        style={{backgroundColor:'red', borderRadius:'5px', }}>
        <BiTrash size={30} color='white'/>
    </button>
  )
}

export default DeleteWaterParameter;