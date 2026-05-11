import { useState } from 'react';
import { skillApi } from '../../api_services/skill/SkillAPIService';
import type { Skill } from '../../models/Skill';
import toast from 'react-hot-toast';

const SkillSearch = () => {
    const [search, setSearch] = useState('');
    const [foundSkill, setFoundSkill] = useState<Skill | null>(null);

    const handleSearchById = async () => {
        if (!search.trim()) {
            toast.error('Please enter a skill ID to search.');
            return;
        }
        setFoundSkill(null);
        const skill = await skillApi.getSkillById(search);
        if (skill) {
            setFoundSkill(skill);
            toast.success('Skill found!');
        } else {
            toast.error('Not found skill with that ID.');
        }
    };

    const handleSearchByName = async () => {
        if (!search.trim()) {
            toast.error('Please enter a skill name to search.');
            return;
        }
        setFoundSkill(null);
        const skill = await skillApi.getSkillByName(search);
        if (skill) {
            setFoundSkill(skill);
            toast.success('Skill found!');
        } else {
            toast.error('Not found skill with that name.');
        }
    };

    return (
        <div style={{ marginBottom: '30px', padding: '15px' }}>
            <h2 style={{color:'black'}}>Search skill</h2>
            <input 
                type="text" 
                value={search} 
                onChange={(e) => setSearch(e.target.value)} 
                placeholder="Enter skill ID or name" 
                style={{ padding: '6px', width: '300px', borderRadius: '4px', border: '1px solid #ccc' }}
            />
            <div style={{ marginTop: '10px' }}/>
            <button onClick={handleSearchById} style={{
                            padding: '6px 12px',
                            cursor: 'pointer',
                            fontSize: '15px',
                            border: 'none',
                            borderRadius: '4px',
                            background: '#007bff',
                            color: 'white',
                            marginRight: '10px'
                        }}>Search by ID</button>
            <button onClick={handleSearchByName} style={{
                            padding: '6px 12px',
                            cursor: 'pointer',
                            fontSize: '15px',
                            border: 'none',
                            borderRadius: '4px',
                            background: '#007bff',
                            color: 'white',
                            marginLeft: '10px'
                        }}>Search by Name</button>

            {foundSkill && (
                <div style={{ 
                    marginTop: '15px', 
                    padding: '12px', 
                    backgroundColor: '#f9f9f9',
                    border: '1px solid #ddd', 
                    borderRadius: '6px',
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center'
                }}>
                    <div >
                        <div style={{ fontWeight: 'bold', color: '#333' }}>{foundSkill.name}</div>
                        <div style={{ fontSize: '12px', color: '#888' }}>ID: {foundSkill.id}</div>
                    </div>
                    
                    <button 
                        onClick={() => {
                            setFoundSkill(null);
                            setSearch("");
                        }}
                        style={{
                            padding: '4px 8px',
                            cursor: 'pointer',
                            fontSize: '11px',
                            border: '1px solid white',
                            borderRadius: '4px',
                            background: '#007bff',
                            color: 'white'
                        }}
                    >
                        Clear
                    </button>
                </div>
            )}
        </div>
    );
};

export default SkillSearch;