import { useState, useEffect } from 'react';
import type { CreateUpdateCandidateRequest, Candidate } from '../../models/Candidate';

interface Props {
    onSubmit: (candidate: CreateUpdateCandidateRequest) => Promise<boolean>;
    initialData?: Candidate | null;
}

const CandidateForm = ({ onSubmit, initialData }: Props) => {
    const emptyForm: CreateUpdateCandidateRequest = {
        fullName: '',
        dateOfBirth: '',
        email: '',
        contactNum: ''
    };

    const [formData, setFormData] = useState<CreateUpdateCandidateRequest>(emptyForm);

    useEffect(() => {
        if (initialData) {
            setFormData({
                fullName: initialData.fullName,
                dateOfBirth: initialData.dateOfBirth ? initialData.dateOfBirth.split('T')[0] : '',
                email: initialData.email,
                contactNum: initialData.contactNum
            });
        } else {
            setFormData(emptyForm);
        }
    }, [initialData]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        const isSuccess = await onSubmit(formData);
        
        if (isSuccess) {
            setFormData(emptyForm);
        }
    };

    return (
        <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '10px', padding: '15px' }}>
            <h2>{initialData ? 'Update' : 'Add'} Candidate</h2>
            <input 
                type="text" placeholder="Full Name" required
                value={formData.fullName ?? ''}
                onChange={e => setFormData({...formData, fullName: e.target.value})}
                style={{ padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}
            />
            <input 
                type="email" placeholder="Email" required
                value={formData.email ?? ''}
                onChange={e => setFormData({...formData, email: e.target.value})}
                style={{ padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}
            />
            <input 
                type="date" required
                value={formData.dateOfBirth ?? ''}  
                onChange={e => setFormData({...formData, dateOfBirth: e.target.value})}
                style={{ padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}
            />
            <input 
                type="text" placeholder="Phone Number" required
                value={formData.contactNum ?? ''}
                onChange={e => setFormData({...formData, contactNum: e.target.value})}
                style={{ padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}
            />
            <button type="submit" style={{
                padding: '10px', 
                background: initialData ? '#ffc107' : '#28a745',
                color: initialData ? 'black' : 'white', 
                border: 'none', 
                borderRadius: '4px', 
                cursor: 'pointer',
                fontWeight: 'bold'
            }}>
                {initialData ? 'Save Changes' : 'Create Candidate'}
            </button>
        </form>
    );
};

export default CandidateForm;