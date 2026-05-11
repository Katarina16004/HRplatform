import React, { useState } from 'react';
import type { CreateSkillRequest } from '../../models/Skill';
import toast from 'react-hot-toast';

interface Props {
    onSkillCreated: (request: CreateSkillRequest) => void;
}

const SkillForm = ({ onSkillCreated }: Props) => {
    const [name, setName] = useState('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!name.trim()) 
        {
            toast.error('Please enter a skill name.');
            return;
        }
        onSkillCreated({ name: name.trim() });
        setName("");
    };

    return (
        <div style={{ marginBottom: '30px', padding: '15px' }}>
            <h2 style={{color:'black'}}>Add new skill</h2>
            <form onSubmit={handleSubmit}>
                <input 
                    type="text" 
                    value={name} 
                    onChange={(e) => setName(e.target.value)} 
                    placeholder="Enter skill name"
                    style={{ padding: '6px', width: '300px', borderRadius: '4px', border: '1px solid #ccc' }}
                />
                <div style={{ marginTop: '10px' }}/>
                <button 
                    type="submit" 
                    style={{
                            padding: '6px 12px',
                            cursor: 'pointer',
                            fontSize: '15px',
                            border: 'none',
                            borderRadius: '4px',
                            background: '#007bff',
                            color: 'white'
                        }}
                >
                    Add skill
                </button>
            </form>
        </div>
    );
};

export default SkillForm;